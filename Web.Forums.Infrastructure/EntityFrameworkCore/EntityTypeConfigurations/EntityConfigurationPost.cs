namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Aggregate;


public class EntityConfigurationPost : IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder.ToTable("Post");

		builder.HasKey(x => x.PostId);
		builder.Property(x => x.PostId)
			.HasConversion(x => x.ToGuid(), x => new Ulid(x))
			;

		builder.HasIndex(x => x.TopicId);
		builder.Property(x => x.TopicId)
			.HasConversion(x => x.ToGuid(), x => new Ulid(x))
			;


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
		}
	}



}
