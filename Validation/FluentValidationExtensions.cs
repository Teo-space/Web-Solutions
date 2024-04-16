using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class FluentValidationExtensions
{
	public static IServiceCollection AddFluentValidationWithValidators(this IServiceCollection services,
		params Assembly[] assemblies)
	{
		services.AddFluentValidationAutoValidation(cf => { cf.DisableDataAnnotationsValidation = true; })
			.AddFluentValidationClientsideAdapters();

		foreach (var assembly in assemblies)
		{
			services.AddValidatorsFromAssembly(assembly);
		}

		return services;
	}
}
