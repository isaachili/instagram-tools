using IsaacHili.InstagramTools.Models.Enums;

namespace IsaacHili.InstagramTools.Models;

/// <summary>
/// An entity representing an account's connection.
/// </summary>
public record class Connection
{
	/// <summary>
	/// Connection's username
	/// </summary>
	public required string Username { get; set; }

	/// <summary>
	/// Connection's profile URI
	/// </summary>
	public required Uri ProfileUri { get; set; }

	/// <summary>
	/// The date when the subject was followed by the connection.
	/// </summary>
	public DateTime DateFollowed { get; set; } = DateTime.MinValue;

	/// <summary>
	/// The date when the connection was followed by the subject.
	/// </summary>
	public DateTime DateFollowing { get; set; } = DateTime.MinValue;

	/// <summary>
	/// Connection type
	/// </summary>
	public ConnectionTypes ConnectionType { get; set; }
}
