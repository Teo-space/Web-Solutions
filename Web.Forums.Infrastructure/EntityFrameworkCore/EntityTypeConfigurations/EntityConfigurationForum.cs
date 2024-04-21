namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Aggregate;
using Web.Forums.Domain.Owned;

public class EntityConfigurationForum : IEntityTypeConfiguration<Forum>
{
	public void Configure(EntityTypeBuilder<Forum> builder)
	{
		builder.ToTable("Forums");


		builder.HasKey(x => x.ForumId);
		builder.Property(x => x.ForumId);//.ValueGeneratedOnAdd();

		builder.HasIndex(x => x.ParentForumId);
		builder.Property(x => x.ParentForumId);

		builder.HasIndex(x => x.Title).IsUnique();
		builder.Property(x => x.Title).HasMaxLength(50).IsConcurrencyToken();

		builder.Property(x => x.Description).HasMaxLength(100);


		{
			builder.OwnsOne(f => f.CreatedBy, owned =>
			{
				owned.Property(x => x.UserId).IsRequired();
				owned.Property(x => x.UserName).IsRequired().HasMaxLength(100);
				owned.Property(x => x.At).IsRequired();
			});

			builder.OwnsOne(f => f.UpdatedBy, owned =>
			{
				owned.Property(x => x.UserId).IsRequired();
				owned.Property(x => x.UserName).IsRequired().HasMaxLength(100);
				owned.Property(x => x.At).IsRequired();
			});

			builder.OwnsOne(f => f.RepliedBy, owned =>
			{
				owned.Property(x => x.UserId).IsRequired();
				owned.Property(x => x.UserName).IsRequired().HasMaxLength(100);
				owned.Property(x => x.At).IsRequired();
			});
		}



		builder.HasMany(f => f.Edits).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.ForumId)
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasMany(f => f.Moderations).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.ForumId)
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasMany(f => f.Curators).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.ForumId)
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasMany(f => f.Moderators).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.ForumId)
			.OnDelete(DeleteBehavior.NoAction);


		builder.HasMany(parent => parent.Forums).WithOne(child => child.ParentForum)
			.HasPrincipalKey(parent => parent.ForumId).HasForeignKey(child => child.ParentForumId)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasMany(f => f.Announcements).WithOne(a => a.Forum)
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(a => a.ForumId)
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasMany(f => f.Topics).WithOne(t => t.Forum)
			.HasPrincipalKey(m => m.ForumId).HasForeignKey(t => t.ForumId)
			.OnDelete(DeleteBehavior.NoAction);


	}
}
