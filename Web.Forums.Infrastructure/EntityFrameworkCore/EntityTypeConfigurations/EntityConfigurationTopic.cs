namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Aggregate;


public class EntityConfigurationTopic : IEntityTypeConfiguration<Topic>
{
	public void Configure(EntityTypeBuilder<Topic> builder)
	{
		builder.ToTable("Topics");

		builder.HasKey(x => x.TopicId);
		builder.Property(x => x.TopicId)
			.HasConversion(x => x.ToGuid(), x => new Ulid(x))
			;

		builder.HasIndex(x => x.ForumId);
		builder.Property(x => x.ForumId)
			.HasConversion(x => x.ToGuid(), x => new Ulid(x))
			;


		builder.HasIndex(x => x.Title).IsUnique();
		builder.Property(x => x.Title).HasMaxLength(100).IsConcurrencyToken();
		builder.Property(x => x.Text).HasMaxLength(500).IsConcurrencyToken();


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

		builder.HasMany(t => t.Posts)
			.WithOne(p => p.Topic)
			.HasPrincipalKey(t => t.TopicId)
			.HasForeignKey(p => p.TopicId)
			.OnDelete(DeleteBehavior.NoAction);



	}



}
