﻿using Identity.Interfaces;

namespace Identity.Infrastructure.Initializers;


public static class RootUserInitializer
{
	public static void Initialize(IServiceProvider serviceProvider)
	{
		using (var scope = serviceProvider.CreateScope())
		{
			var rootUserInitializerService = scope.ServiceProvider.GetRequiredService<IRootUserInitializerService>();
			rootUserInitializerService.InitializeRootUser().Wait();
			rootUserInitializerService.AddRootUserToBasicRoles().Wait();
		}
	}

}
