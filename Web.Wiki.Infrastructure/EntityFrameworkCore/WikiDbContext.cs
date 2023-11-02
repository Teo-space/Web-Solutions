using Web.Wiki.Domain;

namespace Web.Wiki.Infrastructure.EntityFrameworkCore;


public class WikiDbContext : DbContext
{
	public WikiDbContext(DbContextOptions<WikiDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public DbSet<Article> Forums { get; set; }



}


