using Identity.Interfaces;

namespace Identity.Infrastructure.Initializers
{
	public static class BasicRolesInitializer
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var basicRolesInitializerService = scope.ServiceProvider.GetRequiredService<IBasicRolesInitializerService>();
				basicRolesInitializerService.Initialize().Wait();
			}
		}


	}
}
