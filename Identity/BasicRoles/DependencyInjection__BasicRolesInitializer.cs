using Identity.BasicRoles;
using Microsoft.AspNetCore.Builder;

public static class DependencyInjection__BasicRolesInitializer
{

	public static void AddBasicRolesInitializer(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IBasicRolesInitializerService, BasicRolesInitializerService>();
	}

	public static void AddBasicRolesInitializerHostedService(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IBasicRolesInitializerService, BasicRolesInitializerService>();

		builder.Services.AddHostedService<BasicRolesInitializer__HostedService>();
	}

}

