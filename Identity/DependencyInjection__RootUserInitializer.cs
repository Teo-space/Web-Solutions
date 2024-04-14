using Identity.Infrastructure.Services;
using Identity.Interfaces;

public static class DependencyInjection__RootUserInitializer
{
	public static void AddRootUserOptions(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<RootUserOptions>(builder.Configuration.GetSection(nameof(RootUserOptions)));

		builder.Services.AddOptions<RootUserOptions>()
			.Bind(builder.Configuration.GetSection(nameof(RootUserOptions)));

	}

	public static void AddRootUserInitializer(this WebApplicationBuilder builder)
	{
		builder.AddRootUserOptions();
		builder.Services.AddScoped<IRootUserInitializerService, RootUserInitializerService>();
	}

}