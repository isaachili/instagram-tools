using IsaacHili.InstagramTools.Followers.Abstractions.Services;
using IsaacHili.InstagramTools.Followers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IsaacHili.InstagramTools.Followers.Extensions;

public static class FollowersExtensions
{
	public static IServiceCollection AddFollowers(this IServiceCollection services)
	{
		// Inject ConnectionsService
		_ = services.AddSingleton<IConnectionsService, ConnectionsService>();

		return services;
	}
}
