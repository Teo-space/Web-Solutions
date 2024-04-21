using Web.Forums.Domain.Aggregate;
using Web.Forums.Infrastructure.EntityFrameworkCore.Convertors;

namespace Web.Forums.Infrastructure.EntityFrameworkCore;


public class ForumDbContext : DbContext
{
	public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}
/*
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.EnableDetailedErrors();
		optionsBuilder.EnableSensitiveDataLogging();
		optionsBuilder.LogTo(Console.WriteLine, minimumLevel: Microsoft.Extensions.Logging.LogLevel.Information);
	}
*/
	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<Ulid>().HaveConversion<UlidToGuidConvertor>();
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public DbSet<Forum> Forums { get; set; }
	public DbSet<Announcement> Announcements { get; set; }
	public DbSet<Topic> Topics { get; set; }
	public DbSet<Post> Posts { get; set; }




}


