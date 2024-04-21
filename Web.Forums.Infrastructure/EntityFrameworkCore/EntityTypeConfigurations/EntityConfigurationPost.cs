namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Aggregate;


public class EntityConfigurationPost : IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder.ToTable("Post");

		builder.HasKey(x => x.PostId);
		builder.Property(x => x.PostId);

		builder.HasIndex(x => x.TopicId);
		builder.Property(x => x.TopicId);


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
