namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Owned;

public class EntityConfigurationOwnedModerations : IEntityTypeConfiguration<Moderation>
{
	public void Configure(EntityTypeBuilder<Moderation> builder)
	{
		builder.ToTable("Owned.Moderations");

		builder.HasIndex(x => x.OwnerId);
		builder.Property(x => x.OwnerId).HasConversion(x => x.ToGuid(), x => new Ulid(x));

		builder.HasKey(x => x.ModerationId);
		builder.Property(x => x.ModerationId).HasConversion(x => x.ToGuid(), x => new Ulid(x));

		builder.Property(x => x.ModeratedByUserId).IsRequired();
		builder.Property(x => x.ModeratedByUserName).IsRequired();
		builder.Property(x => x.ActionName).IsRequired().HasMaxLength(100);
		builder.Property(x => x.ObjectId).IsRequired().HasConversion(x => x.ToGuid(), x => new Ulid(x));
		builder.Property(x => x.Comment).HasMaxLength(500);



	}
}
