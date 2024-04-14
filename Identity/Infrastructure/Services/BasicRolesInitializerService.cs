using Identity.Interfaces;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Services;


internal class BasicRolesInitializerService(ILogger<BasicRolesInitializerService> logger, RoleManager roleManager)

	: IBasicRolesInitializerService
{

	public async Task<List<string>> Initialize()
	{
		List<string> errors = new List<string>();

		foreach (var roleName in Enum.GetNames<BasicRoles>())
		{
			if (!await roleManager.RoleExistsAsync(roleName))
			{
				var result = await roleManager.CreateAsync(new(roleName));
				if (result.Succeeded)
				{
					logger.LogInformation($"Role Created: {roleName}");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						logger.LogError($"Role {roleName} Creating finished with error: {error.Description}");
						errors.Add($"Role {roleName} Creating finished with error: {error.Description} </br>");
					}
				}
			}
		}
		return errors;
	}



}
