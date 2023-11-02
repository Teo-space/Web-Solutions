using Identity.RootUser;




public class RootUserInitializer__HostedService(
	IServiceProvider serviceProvider,
	ILogger<RootUserInitializer__HostedService> logger)

	: IHostedService
{
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Started");

		using (var scope = serviceProvider.CreateScope())
		{
			var rootUserInitializer = scope.ServiceProvider.GetRequiredService<IRootUserInitializerService>();
			var errors = await rootUserInitializer.InitializeRootUser();
			await rootUserInitializer.AddRootUserToBasicRoles();
		}

		logger.LogInformation("Finished");
	}

	public async Task Scope(IServiceScope serviceScope)
	{
		var UserOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<RootUserOptions>>();
		if(UserOptions.Value  != null || string.IsNullOrEmpty(UserOptions?.Value?.Email))
		{
			logger.LogError("UserOptions.Value.Email is null or empty!!!!");
			return;
		}

		var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager>();
		var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager>();

		//while (!await roleManager.RoleExistsAsync(BasicRoles.Admin.ToString()))
		//{
		//	await Task.Delay(3000);
		//	logger.LogInformation("Awaiting init Basic Roles");
		//}

		if(await userManager.FindByEmailAsync(UserOptions.Value.Email) != null)
		{
			logger.LogInformation("root user exists");
			return;
		}

		var result = await userManager.CreateAsync(new User()
		{
			UserName = UserOptions.Value.UserName,
			Email = UserOptions.Value.Email,
		}, UserOptions.Value.Password);

		if(result.Succeeded)
		{
			logger.LogInformation("root user created");
		}
		else
		{
			foreach(var e  in result.Errors)
			{
				logger.LogError(e.Description);
			}
		}
	}


	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}


}