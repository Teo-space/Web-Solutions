using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Web.Forums.Infrastructure.EntityFrameworkCore;

public static class DependencyInjection__ForumsInfrastructure
{
	private static void AddForumInfrastructureInternal(this IServiceCollection services)
	{
		services.AddLogging();

		services.AddFluentValidationAutoValidation();
		services.AddFluentValidationClientsideAdapters();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public static void AddForumTestingInfrastructure(this IServiceCollection services)
	{
		services.AddForumInfrastructureInternal();

		services.AddDbContext<ForumDbContext>(options =>
		{
			options.UseInMemoryDatabase("ForumTest");
		});
	}


	public static void AddForumInfrastructureUseMariaDb(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddForumInfrastructureInternal();

		string ForumConnectionStringName = "MariaDbConnectionForum";
		var connectionString = configuration.GetConnectionString(ForumConnectionStringName)
			?? throw new InvalidOperationException($"Connection string '{ForumConnectionStringName}' not found.");

		services.AddDbContext<ForumDbContext>(options =>
		{
			options.UseMySql(connectionString, ServerVersion.Create(new Version(11, 3, 2), ServerType.MariaDb));
		});

	}

	public static void AddForumInfrastructureUseMySql(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddForumInfrastructureInternal();

		string ForumConnectionStringName = "MariaDbConnectionForum";
		var connectionString = configuration.GetConnectionString(ForumConnectionStringName)
			?? throw new InvalidOperationException($"Connection string '{ForumConnectionStringName}' not found.");

		services.AddDbContext<ForumDbContext>(options =>
		{
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
		});

	}

	public static void AddForumInfrastructureUseNpgsql(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddForumInfrastructureInternal();

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		string ForumConnectionStringName = "PsqlConnectionForum";
		var connectionString = configuration.GetConnectionString(ForumConnectionStringName)
			?? throw new InvalidOperationException($"Connection string '{ForumConnectionStringName}' not found.");

		services.AddDbContext<ForumDbContext>(options => options
			.UseNpgsql(connectionString));
	}


	public static void AddForumInfrastructureUseSqlServer(this WebApplicationBuilder builder, string connectionString)
	{
		builder.Services.AddForumInfrastructureInternal();

		builder.Services.AddDbContext<ForumDbContext>(options => options
			.UseSqlServer(connectionString));
	}



}