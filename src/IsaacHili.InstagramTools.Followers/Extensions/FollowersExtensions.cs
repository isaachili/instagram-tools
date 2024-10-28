using IsaacHili.InstagramTools.Followers.Abstractions.Services;
using IsaacHili.InstagramTools.Followers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IsaacHili.InstagramTools.Followers.Extensions;

public static class FollowersExtensions
{
	/// <summary>
	/// Adds the services for the followers/connections module.
	/// </summary>
	/// <param name="services">Service collection</param>
	/// <returns>
	/// The <paramref name="services"/> service collection instance.
	/// </returns>
	public static IServiceCollection AddFollowers(this IServiceCollection services)
	{
		// Inject ConnectionsService
		_ = services.AddSingleton<IConnectionsService, ConnectionsService>();

		return services;
	}
}
