namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Aggregate;
using Web.Forums.Domain.Owned;

public class EntityConfigurationForum : IEntityTypeConfiguration<Forum>
{
	public void Configure(EntityTypeBuilder<Forum> builder)
	{
		builder.ToTable("Forums");


		builder.HasIndex(x => x.ForumId).IsDescending(true).IsClustered(true);
		builder.Property(x => x.ForumId)
			.HasConversion(x => x.ToGuid(), x => new Ulid(x));
		;
		builder.HasIndex(x => x.ParentForumId);
		builder.Property(x => x.ParentForumId)
			.HasConversion(x => x.HasValue ? x.Value.ToGuid() : default, x => new Ulid(x));
			;

		builder.HasIndex(x => x.Title).IsUnique();
		builder.Property(x => x.Title).HasMaxLength(50).IsConcurrencyToken();

		builder.Property(x => x.Description).HasMaxLength(100).IsConcurrencyToken();

		builder.HasIndex(x => x.Closed);
		builder.HasIndex(x => x.Deleted);

		{
			builder.OwnsOne(f => f.CreatedBy, owned =>
			{
				owned.HasIndex(x => x.UserId);
				owned.Property(x => x.UserId).IsRequired();

				owned.HasIndex(x => x.UserName);
				owned.Property(x => x.UserName).IsRequired();

				owned.HasIndex(x => x.At);
				owned.Property(x => x.At).IsRequired();
			});

			builder.OwnsOne(f => f.UpdatedBy, owned =>
			{
				owned.HasIndex(x => x.UserId);
				owned.Property(x => x.UserId).IsRequired();

				owned.HasIndex(x => x.UserName);
				owned.Property(x => x.UserName).IsRequired();

				owned.HasIndex(x => x.At);
				owned.Property(x => x.At).IsRequired();
			});

			builder.OwnsOne(f => f.RepliedBy, owned =>
			{
				owned.HasIndex(x => x.UserId);
				owned.Property(x => x.UserId).IsRequired();

				owned.HasIndex(x => x.UserName);
				owned.Property(x => x.UserName).IsRequired();

				owned.HasIndex(x => x.At);
				owned.Property(x => x.At).IsRequired();
			});
		}



		builder.HasMany(f => f.Edits).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.OwnerId)
			.OnDelete(DeleteBehavior.NoAction);
		//builder.Navigation(f => f.Edits).AutoInclude();

		builder.HasMany(f => f.Moderations).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.OwnerId)
			.OnDelete(DeleteBehavior.NoAction);
		//builder.Navigation(f => f.Moderations).AutoInclude();


		builder.HasMany(f => f.Curators).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.ParentId)
			.OnDelete(DeleteBehavior.NoAction);
		//builder.Navigation(f => f.Curators).AutoInclude();

		builder.HasMany(f => f.Moderators).WithOne()
			.HasPrincipalKey(f => f.ForumId).HasForeignKey(e => e.ParentId)
			.OnDelete(DeleteBehavior.NoAction);
		//builder.Navigation(f => f.Moderators).AutoInclude();




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
