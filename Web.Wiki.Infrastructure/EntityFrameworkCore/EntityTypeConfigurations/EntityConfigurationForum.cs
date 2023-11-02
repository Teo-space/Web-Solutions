using Web.Wiki.Domain;

namespace Web.Wiki.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;
public class EntityConfigurationForum : IEntityTypeConfiguration<Article>
{
	public void Configure(EntityTypeBuilder<Article> builder)
	{
		builder.ToTable("Articles");

		builder.HasIndex(x => x.ArticleId);
		builder.Property(x => x.ArticleId)
			.HasConversion(x => x.ToGuid(), x => new Ulid(x));


		builder.HasIndex(x => x.ArticleVersionId).IsDescending(true).IsClustered(true);
		builder.Property(x => x.ArticleVersionId)
			.HasConversion(x => x.ToGuid(), x => new Ulid(x));


		builder.HasIndex(x => x.Title).IsUnique();
		builder.Property(x => x.Title).HasMaxLength(50).IsConcurrencyToken();


		builder.Property(x => x.Description).HasMaxLength(100).IsConcurrencyToken();


		builder.Property(x => x.Text).HasMaxLength(10000).IsConcurrencyToken();


		builder.HasIndex(x => x.Approved);
		builder.HasIndex(x => x.BaseVersion);
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
			builder.OwnsOne(f => f.ApprovedBy, owned =>
			{
				owned.HasIndex(x => x.UserId);
				owned.Property(x => x.UserId).IsRequired();

				owned.HasIndex(x => x.UserName);
				owned.Property(x => x.UserName).IsRequired();

				owned.HasIndex(x => x.At);
				owned.Property(x => x.At).IsRequired();
			});
			builder.OwnsOne(f => f.BaseVersionBy, owned =>
			{
				owned.HasIndex(x => x.UserId);
				owned.Property(x => x.UserId).IsRequired();

				owned.HasIndex(x => x.UserName);
				owned.Property(x => x.UserName).IsRequired();

				owned.HasIndex(x => x.At);
				owned.Property(x => x.At).IsRequired();
			});
			builder.OwnsOne(f => f.DeletedBy, owned =>
			{
				owned.HasIndex(x => x.UserId);
				owned.Property(x => x.UserId).IsRequired();

				owned.HasIndex(x => x.UserName);
				owned.Property(x => x.UserName).IsRequired();

				owned.HasIndex(x => x.At);
				owned.Property(x => x.At).IsRequired();
			});


		}


		builder.HasMany(f => f.ArticleVersions).WithOne(t => t.Main)
			.HasPrincipalKey(m => m.ArticleId).HasForeignKey(t => t.ArticleId)
			.OnDelete(DeleteBehavior.NoAction);


	}
}
