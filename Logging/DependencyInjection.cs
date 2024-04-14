using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


public static class DependencyInjection
{
	public static ILoggingBuilder AddSerilogLogging(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
	{
		var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(configuration);
		var logger = loggerConfiguration.CreateLogger();

		loggingBuilder.ClearProviders();
		loggingBuilder.AddSerilog(logger);

		return loggingBuilder;
	}

	public static IHostBuilder AddSerilogLogging(this IHostBuilder builder)
	{
		builder.UseSerilog((context, services, configuration) => configuration
			.ReadFrom.Configuration(context.Configuration)
			.ReadFrom.Services(services)
			.Enrich.FromLogContext()
			.WriteTo.Console()
			.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
			.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
			.MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
			);

		return builder;
	}


}
