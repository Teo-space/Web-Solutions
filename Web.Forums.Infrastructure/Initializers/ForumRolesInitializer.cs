using Microsoft.Extensions.Options;
using Web.Forums.Domain.Enums;

namespace Web.Forums.Infrastructure.Initializers;

public static class ForumRolesInitializer
{
	public static void Initialize(IServiceProvider serviceProvider)
	{
		using (var scope = serviceProvider.CreateScope())
		{
			//var forumDbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

			RoleManager roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();

			foreach (var roleName in Enum.GetValues<ForumRoles>())
			{
				InitialiseRole(roleManager, roleName);
			}


			IOptions<RootUserOptions> UserOptions = scope.ServiceProvider.GetRequiredService<IOptions<RootUserOptions>>();
			UserManager userManager = scope.ServiceProvider.GetRequiredService<UserManager>();

			var user = userManager.FindByNameAsync(UserOptions.Value.UserName).GetAwaiter().GetResult();
			foreach (var roleName in Enum.GetNames<ForumRoles>().Where(x => x != ForumRoles.ForumRoleBanned.ToString()))
			{
				if(!userManager.IsInRoleAsync(user, roleName).GetAwaiter()!.GetResult())
				{
					userManager.AddToRoleAsync(user, roleName).GetAwaiter()!.GetResult();
				}	
			}
		}
	}

	static void InitialiseRole(RoleManager roleManager, ForumRoles roles)
	{
		if (!roleManager.RoleExistsAsync(roles.ToString()).GetAwaiter().GetResult())
		{
			var result = roleManager.CreateAsync(new(roles.ToString())).GetAwaiter().GetResult();
			if (result.Succeeded)
			{
				//logger.LogInformation($"Role Created: {roleName}");
			}
			else
			{

			}
		}
	}

}
