namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Owned;

public class EntityConfigurationOwnedModerators : IEntityTypeConfiguration<Moderator>
{
	public void Configure(EntityTypeBuilder<Moderator> builder)
	{
		builder.ToTable("Owned.Moderators");

		builder.HasIndex(x => x.ParentId);
		builder.Property(x => x.ParentId);

		builder.HasKey(x => x.ModeratorId);
		builder.Property(x => x.ModeratorId).IsRequired();
		builder.Property(x => x.UserId).IsRequired();
		builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
	}
}
