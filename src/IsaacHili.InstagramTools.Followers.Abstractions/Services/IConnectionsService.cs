namespace IsaacHili.InstagramTools.Followers.Abstractions.Services;

/// <summary>
/// A service responsible for processing accounts followed and account followers.
/// </summary>
public interface IConnectionsService
{
	/// <summary>
	/// The subject and the connections follow each other.
	/// </summary>
	IEnumerable<Connection> Mutual { get; }

	/// <summary>
	/// Connections that are followed by the subject, but are not followed back.
	/// E.g. You reading this and @sydney_sweeney.
	/// </summary>
	IEnumerable<Connection> NotFollowed { get; }

	/// <summary>
	/// Connections that follow the subject, but are not followed back.
	/// </summary>
	IEnumerable<Connection> NotFollowing { get; }

	/// <summary>
	/// Loads and processes the followers and followed accounts from the input stream.
	/// </summary>
	/// <param name="stream">Input stream</param>
	Task LoadConnectionsAsync(Stream stream);
}
