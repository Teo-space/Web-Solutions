namespace Identity.RootUser;




internal class RootUserInitializerService(
			ILogger<RootUserInitializerService> logger,

			RoleManager roleManager,
			IOptions<RootUserOptions> UserOptions,
			UserManager userManager

)	: IRootUserInitializerService
{

	public async Task<List<string>> InitializeRootUser()
	{
		List<string> errors = new List<string>();

		if (string.IsNullOrEmpty(UserOptions?.Value?.Email))
		{
			logger.LogError("UserOptions.Value.Email is null or empty!!!!");
			errors.Add("UserOptions.Value.Email is null or empty!!!!");
			return errors;
		}

		if (await userManager.FindByEmailAsync(UserOptions.Value.Email) != null)
		{
			logger.LogInformation("root user exists");
			return errors;
		}

		var result = await userManager.CreateAsync(new User()
		{
			Id = Guid.Empty,
			UserName = UserOptions.Value.UserName,
			Email = UserOptions.Value.Email,
		}, UserOptions.Value.Password);

		if (result.Succeeded)
		{
			logger.LogInformation("root user created");

			var user = await userManager.FindByEmailAsync(UserOptions.Value.Email);
			var userId = await userManager.GetUserIdAsync(user);
			var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

			var ConfirmEmailResult = await userManager.ConfirmEmailAsync(user, code);
			if (ConfirmEmailResult.Succeeded)
			{
				logger.LogInformation("ConfirmEmail Succeeded");
			}
			else
			{
				foreach (var e in ConfirmEmailResult.Errors)
				{
					logger.LogError(e.Description);
					errors.Add($"userManager.ConfirmEmail finished with error : {e.Description}");
				}
			}
		}
		else
		{
			foreach (var e in result.Errors)
			{
				logger.LogError(e.Description);
				errors.Add($"userManager.Create finished with error : {e.Description}");
			}
		}
		return errors;
	}




	public async Task<List<string>> AddRootUserToBasicRoles()
	{
		List<string> errors = new List<string>();

		while (!await roleManager.RoleExistsAsync(global::BasicRoles.Admin.ToString()))
		{
			await Task.Delay(3000);
			logger.LogInformation("Awaiting init Basic Roles");
		}

		var user = await userManager.FindByEmailAsync(UserOptions.Value.Email);
		if(user == null)
		{
			logger.LogInformation("root user exists");
			errors.Add("Root user not exists!!");
			return errors;
		}


		foreach (var role in Enum.GetNames<global::BasicRoles>())
		{
			if (!await userManager.IsInRoleAsync(user, role))
			{
				var result = await userManager.AddToRoleAsync(user, role);
				if (result.Succeeded)
				{
					logger.LogInformation($"root user added to role {role}");
				}
				else
				{
					foreach (var e in result.Errors)
					{
						logger.LogError($"userManager.AddToRole finished with error : {e.Description}");
						errors.Add($"userManager.AddToRole finished with error : {e.Description}");
					}
				}

			}
		}

		return errors;
	}




}
