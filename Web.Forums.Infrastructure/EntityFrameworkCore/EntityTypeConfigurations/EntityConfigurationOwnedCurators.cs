namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Owned;

public class EntityConfigurationOwnedCurators : IEntityTypeConfiguration<Curator>
{
	public void Configure(EntityTypeBuilder<Curator> builder)
	{
		builder.ToTable("Owned.Curators");

		builder.HasKey(x => x.CuratorId);
		builder.Property(x => x.CuratorId);//.ValueGeneratedOnAdd();

		builder.HasIndex(x => x.ForumId);
		builder.Property(x => x.ForumId);



		builder.Property(x => x.UserId).IsRequired();
		builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
	}
}
