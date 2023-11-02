using Identity.BasicRoles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


public class BasicRolesInitializer__HostedService(
    IServiceProvider serviceProvider, 
    ILogger<BasicRolesInitializer__HostedService> logger) 

	: IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Started");

        using (var scope = serviceProvider.CreateScope())
        {
			var rolesInitializer = scope.ServiceProvider.GetRequiredService<IBasicRolesInitializerService>();
			var errors = await rolesInitializer.Initialize();
		}

        logger.LogInformation("Finished");
    }


	public Task StopAsync(CancellationToken cancellationToken) 
    {
        return Task.CompletedTask;
    }


}