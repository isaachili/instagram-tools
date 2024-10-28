using System.IO.Compression;
using System.Text.Json.Nodes;
using IsaacHili.InstagramTools.Followers.Abstractions;
using IsaacHili.InstagramTools.Followers.Abstractions.Enums;
using IsaacHili.InstagramTools.Followers.Abstractions.Services;

namespace IsaacHili.InstagramTools.Followers.Services;

internal class ConnectionsService : IConnectionsService
{
	// Connections
	private Connection[] _mutual = [];
	private Connection[] _notFollowed = [];
	private Connection[] _notFollowing = [];

	#region Properties

	public IEnumerable<Connection> Mutual => _mutual;

	public IEnumerable<Connection> NotFollowed => _notFollowed;

	public IEnumerable<Connection> NotFollowing => _notFollowing;

	#endregion

	/// <summary>
	/// Resets the connections.
	/// </summary>
	private void ResetConnections()
	{
		_mutual = [];
		_notFollowed = [];
		_notFollowing = [];
	}

	public async Task LoadConnectionsAsync(Stream stream)
	{
		// Reset connections
		ResetConnections();

		// Load archive
		using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

		// Read entries
		var entries = await LoadArchiveEntriesAsync(archive);

		if (!entries.HasValue)
		{
			return;
		}

		var following = entries.Value.Following["relationships_following"]?.AsArray();
		var followers = entries.Value.Followers.AsArray();

		if (following is null)
		{
			return;
		}

		var connections = new Dictionary<string, Connection>();

		// Deserialize entries
		DeserializeJsonArray(connections, following, ConnectionTypes.Following);
		DeserializeJsonArray(connections, followers, ConnectionTypes.Follower);

		// Sort connections
		Sort(connections);
	}

	/// <summary>
	/// Gets or creates a <see cref="Connection"/> instance for the requested username.
	/// </summary>
	/// <param name="connections">Connections</param>
	/// <param name="username">Account's handle</param>
	/// <param name="href">Account's profile URL</param>
	/// <param name="connectionType">Connection type</param>
	/// <param name="timestamp">Follow timestamp</param>
	/// <returns>
	/// The <see cref="Connection"/> instance attributed to an account.
	/// </returns>
	private static Connection GetOrAddConnection(Dictionary<string, Connection> connections,
		string username, string href, ConnectionTypes connectionType, DateTime timestamp)
	{
		// Get or add connection
		if (!connections.TryGetValue(username, out var connection))
		{
			connection = new Connection
			{
				Username = username,
				ProfileUri = new Uri(href, UriKind.Absolute),
			};

			connections.Add(username, connection);
		}

		// Set connection type
		connection.ConnectionType |= connectionType;

		// Set timestamp
		switch (connectionType)
		{
			case ConnectionTypes.Following:
				connection.DateFollowing = timestamp;
				break;

			case ConnectionTypes.Follower:
				connection.DateFollowed = timestamp;
				break;
		}

		return connection;
	}

	/// <summary>
	/// Deserializes a JSON array to a <see cref="Connection"/> instance,
	/// and appends the connection to <paramref name="connections"/>.
	/// </summary>
	/// <param name="connections">Connections</param>
	/// <param name="root">Root JSON array</param>
	/// <param name="connectionType">Connection type</param>
	private static void DeserializeJsonArray(Dictionary<string, Connection> connections,
		JsonArray root, ConnectionTypes connectionType)
	{
		foreach (var node in root)
		{
			if (node is null)
			{
				continue;
			}

			var @object = node.AsObject();

			// Read string_list_data
			var listData = @object["string_list_data"]?.AsArray();

			if (listData is not { Count: > 0 })
			{
				continue;
			}

			var data = listData[0];

			// Read username
			string? username = data?["value"]?.GetValue<string>();
			
			// Read href
			string? href = data?["href"]?.GetValue<string>();

			// Read timestamp
			long? timestampSeconds = data?["timestamp"]?.GetValue<long>();

			if (username is null || href is null || !timestampSeconds.HasValue)
			{
				continue;
			}

			// Create DateTime from Unix timestamp
			var timestamp = DateTimeOffset
				.FromUnixTimeSeconds(timestampSeconds.Value)
				.DateTime;

			// Add connection to collection
			GetOrAddConnection(connections, username, href, connectionType, timestamp);
		}
	}

	/// <summary>
	/// Groups connections by connection type and sorts them by follow date.
	/// </summary>
	/// <param name="connections">Connections</param>
	private void Sort(Dictionary<string, Connection> connections)
	{
		// Group and sort connections
		var buckets = connections.Values.GroupBy(g => g.ConnectionType)
			.Where(g => g.Key != ConnectionTypes.None)
			.OrderBy(g => g.Key)
			.Select(g => g
				.OrderBy(c => (IComparable)(c.ConnectionType switch
					{
						ConnectionTypes.Following => c.DateFollowing,
						ConnectionTypes.Follower => c.DateFollowed,
						_ => c.Username,
					}))
				.ToArray())
			.ToArray();

		_notFollowed = buckets[(int)ConnectionTypes.Following - 1];
		_notFollowing = buckets[(int)ConnectionTypes.Follower - 1];
		_mutual = buckets[(int)ConnectionTypes.Mutual - 1];
	}

	#region  Load Archive

	/// <summary>
	/// Loads the followers and followed accounts JSON entries from the archive.
	/// </summary>
	/// <remarks>
	/// Returns null when one of the JSON entries is not found.
	/// </remarks>
	/// <param name="archive">ZIP archive</param>
	/// <returns>
	/// The retrieved followers and following JSON root nodes.
	/// </returns>
	private static async Task<(JsonNode Followers, JsonNode Following)?> LoadArchiveEntriesAsync(ZipArchive archive)
	{
		// Start both tasks simultaneously
		Task<JsonNode?>[] tasks =
		[
			// Load followers
			LoadArchiveEntryAsync(archive, "connections/followers_and_following/followers_1.json"),

			// Load following
			LoadArchiveEntryAsync(archive, "connections/followers_and_following/following.json"),
		];

		// Wait for first task to finish
		var task = await Task.WhenAny(tasks);

		// Exit as soon as possible when null
		if (task.Result is null)
		{
			return null;
		}

		// Wait for both tasks to finish
		await Task.WhenAll(tasks);

		var followers = tasks[0].Result;
		var following = tasks[1].Result;

		// Ensure that both entries were found
		if (followers is null || following is null)
		{
			return null;
		}

		return (followers, following);
	}

	/// <summary>
	/// Reads and parses and entry from the archive.
	/// </summary>
	/// <param name="archive">ZIP archive</param>
	/// <param name="entryName">Entry name</param>
	/// <returns>
	/// The retrieved entry's JSON.
	/// </returns>
	private static async Task<JsonNode?> LoadArchiveEntryAsync(ZipArchive archive, string entryName)
	{
		var entry = archive.GetEntry(entryName);

		if (entry is null)
		{
			return null;
		}

		// Read and parse archive entry
		using var stream = entry.Open();
		var node = await JsonNode.ParseAsync(stream);

		return node;
	}

	#endregion
}
