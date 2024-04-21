namespace Web.Forums.Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

using Web.Forums.Domain.Owned;

public class EntityConfigurationOwnedModerations : IEntityTypeConfiguration<Moderation>
{
	public void Configure(EntityTypeBuilder<Moderation> builder)
	{
		builder.ToTable("Owned.Moderations");

		builder.HasKey(x => x.ModerationId);
		builder.Property(x => x.ModerationId);//.ValueGeneratedOnAdd();

		builder.HasIndex(x => x.ForumId);
		builder.Property(x => x.ForumId);



		builder.Property(x => x.UserId).IsRequired();
		builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);

		builder.Property(x => x.ActionName).IsRequired().HasMaxLength(100);
		builder.Property(x => x.ObjectId).IsRequired();
		builder.Property(x => x.Comment).HasMaxLength(500);



	}
}
