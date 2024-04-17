namespace Identity.Infrastructure.Initializers;

public static class IdentDbContextInitializer
{
	public static void Initialize(IServiceProvider serviceProvider)
	{
		using (var scope = serviceProvider.CreateScope())
		{
			var identDbContext = scope.ServiceProvider.GetRequiredService<IdentDbContext>();
			identDbContext.Database.EnsureCreated();
			identDbContext.SaveChanges();
		}
	}
}
