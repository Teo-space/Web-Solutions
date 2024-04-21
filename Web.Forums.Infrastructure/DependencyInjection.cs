using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
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


	public static void AddForumInfrastructureUseMariaDb(this WebApplicationBuilder builder, string connectionString)
	{
		builder.Services.AddForumInfrastructureInternal();

		builder.Services.AddDbContext<ForumDbContext>(options =>
		{
			options.UseMySql(connectionString, ServerVersion.Create(new Version(11, 3, 2), ServerType.MariaDb));
		});

	}

	public static void AddForumInfrastructureUseMySql(this WebApplicationBuilder builder, string connectionString)
	{
		builder.Services.AddForumInfrastructureInternal();

		builder.Services.AddDbContext<ForumDbContext>(options =>
		{
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
		});

	}

	public static void AddForumInfrastructureUseNpgsql(this WebApplicationBuilder builder, string connectionString)
	{
		builder.Services.AddForumInfrastructureInternal();

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		builder.Services.AddDbContext<ForumDbContext>(options => options
			.UseNpgsql(connectionString));
	}


	public static void AddForumInfrastructureUseSqlServer(this WebApplicationBuilder builder, string connectionString)
	{
		builder.Services.AddForumInfrastructureInternal();

		builder.Services.AddDbContext<ForumDbContext>(options => options
			.UseSqlServer(connectionString));
	}



}