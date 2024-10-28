namespace IsaacHili.InstagramTools.Followers.Abstractions.Enums;

/// <summary>
/// Connection type
/// </summary>
[Flags]
public enum ConnectionTypes
{
	/// <summary>
	/// No connection
	/// </summary>
	None = 0,
	
	/// <summary>
	/// Subject is following the connection.
	/// </summary>
	Following = 1,
	
	/// <summary>
	/// Subject is followed by the connecion.
	/// </summary>
	Follower = 2,
	
	/// <summary>
	/// Subject is followed by the connection,
	/// and is also following the connection.
	/// </summary>
	Mutual = Following | Follower,
}
