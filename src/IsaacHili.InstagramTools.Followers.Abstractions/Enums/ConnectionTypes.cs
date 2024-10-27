namespace IsaacHili.InstagramTools.Followers.Abstractions.Enums;

[Flags]
public enum ConnectionTypes
{
	None = 0,
	Following = 1,
	Follower = 2,
	Mutual = Following | Follower,
}
