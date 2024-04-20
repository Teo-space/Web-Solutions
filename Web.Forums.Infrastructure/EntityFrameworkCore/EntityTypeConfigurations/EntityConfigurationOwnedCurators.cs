namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Owned;

public class EntityConfigurationOwnedCurators : IEntityTypeConfiguration<Curator>
{
	public void Configure(EntityTypeBuilder<Curator> builder)
	{
		builder.ToTable("Owned.Curators");

		builder.HasIndex(x => x.ParentId);
		builder.Property(x => x.ParentId);

		builder.HasKey(x => x.CuratorId);
		builder.Property(x => x.CuratorId).IsRequired();

		builder.Property(x => x.UserId).IsRequired();
		builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
	}
}
