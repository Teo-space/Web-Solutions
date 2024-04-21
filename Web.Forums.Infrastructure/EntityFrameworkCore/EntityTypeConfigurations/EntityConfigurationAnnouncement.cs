namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Aggregate;


public class EntityConfigurationAnnouncement : IEntityTypeConfiguration<Announcement>
{
	public void Configure(EntityTypeBuilder<Announcement> builder)
	{
		builder.ToTable("Announcements");

		builder.HasKey(x => x.AnnouncementId);
		builder.Property(x => x.AnnouncementId);

		builder.HasIndex(x => x.ForumId);
		builder.Property(x => x.ForumId);


		builder.HasIndex(x => x.Title).IsUnique();
		builder.Property(x => x.Title).HasMaxLength(50).IsConcurrencyToken();
		builder.Property(x => x.Text).HasMaxLength(500).IsConcurrencyToken();


		{
			builder.OwnsOne(f => f.CreatedBy, owned =>
			{
				owned.Property(x => x.UserId).IsRequired();
				owned.Property(x => x.UserName).IsRequired();
				owned.Property(x => x.At).IsRequired();
			});

			builder.OwnsOne(f => f.UpdatedBy, owned =>
			{
				owned.Property(x => x.UserId).IsRequired();
				owned.Property(x => x.UserName).IsRequired();
				owned.Property(x => x.At).IsRequired();
			});
		}

	}



}
