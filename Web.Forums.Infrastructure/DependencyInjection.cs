using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Forums.Infrastructure.EntityFrameworkCore;

public static class DependencyInjection__ForumsInfrastructure
{
	public static void AddForumInfrastructure(this WebApplicationBuilder builder, string connectionString)
	{
		builder.Services.AddLogging();

		builder.Services.AddFluentValidationAutoValidation();
		builder.Services.AddFluentValidationClientsideAdapters();
		builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		builder.Services.AddDbContext<ForumDbContext>(options =>
		{
			//options.UseSqlServer(connectionString)
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
			
		});

	}

	public static void AddForumTestingInfrastructure(this IServiceCollection services)
	{
		services.AddLogging();

		services.AddFluentValidationAutoValidation();
		services.AddFluentValidationClientsideAdapters();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


		services.AddDbContext<ForumDbContext>(options =>
		{
			options.UseInMemoryDatabase("ForumTest");
		});

	}

}