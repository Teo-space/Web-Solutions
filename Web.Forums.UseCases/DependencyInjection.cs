using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class DependencyInjection__ForumsUseCases
{
	public static void AddForumUseCases(this WebApplicationBuilder builder)
	{
		builder.Services.AddFluentValidationAutoValidation();
		builder.Services.AddFluentValidationClientsideAdapters();
		builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

		builder.Services.AddUserAccessor();
	}

	public static void AddForumUseCases(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddFluentValidationClientsideAdapters();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

		services.AddUserAccessor();
	}

}
