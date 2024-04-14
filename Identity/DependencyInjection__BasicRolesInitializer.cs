using Identity.Infrastructure.Services;
using Identity.Interfaces;
using Microsoft.AspNetCore.Builder;

public static class DependencyInjection__BasicRolesInitializer
{

	public static void AddBasicRolesInitializer(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IBasicRolesInitializerService, BasicRolesInitializerService>();
	}
}

