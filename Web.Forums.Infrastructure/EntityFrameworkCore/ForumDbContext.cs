using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Infrastructure.EntityFrameworkCore;


public class ForumDbContext : DbContext
{
	public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
	{
		Database.EnsureCreated();
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


