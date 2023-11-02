using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Web.Wiki.Infrastructure.EntityFrameworkCore;

public static class DependencyInjection__ForumsInfrastructure
{
	public static void AddWikiInfrastructure(this WebApplicationBuilder builder)
	{
		builder.Services.AddLogging();

		builder.Services.AddFluentValidationAutoValidation();
		builder.Services.AddFluentValidationClientsideAdapters();
		builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


		builder.Services.AddDbContext<WikiDbContext>(options =>
		{
			options.UseSqlite($"FileName=Data/WikiDbContext.db");
			//options.UseSqlServer(connectionString)
		});

	}

	public static void AddWikiTestingInfrastructure(this IServiceCollection services)
	{
		services.AddLogging();

		services.AddFluentValidationAutoValidation();
		services.AddFluentValidationClientsideAdapters();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


		services.AddDbContext<WikiDbContext>(options =>
		{
			options.UseInMemoryDatabase("WikiDbContext");
		});

	}

}