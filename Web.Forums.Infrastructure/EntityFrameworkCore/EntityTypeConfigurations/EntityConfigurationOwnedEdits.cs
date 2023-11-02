namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Owned;

public class EntityConfigurationOwnedEdits : IEntityTypeConfiguration<Edits>
{
	public void Configure(EntityTypeBuilder<Edits> builder)
	{
		builder.ToTable("Owned.Edits");

		builder.HasIndex(x => x.OwnerId);
		builder.Property(x => x.OwnerId).HasConversion(x => x.ToGuid(), x => new Ulid(x));

		builder.HasKey(x => x.EditsId);
		builder.Property(x => x.EditsId).HasConversion(x => x.ToGuid(), x => new Ulid(x));

		builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
		builder.Property(x => x.Text).IsRequired().HasMaxLength(500);

		builder.Property(x => x.UserId).IsRequired();
		builder.Property(x => x.UserName).IsRequired();


	}
}
