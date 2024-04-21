namespace Web.Forums.Domain.Aggregate;

public sealed partial class Announcement
{
	public static Announcement Create(Forum Forum, PrincipalUser user, string Title, string Text)
	{
		var announcement = new Announcement();
		announcement.Forum = Forum;
		announcement.ForumId = Forum.ForumId;

		announcement.AnnouncementId = IdentityType.NewUlid();

		announcement.Title = Title;
		announcement.Text = Text;

		announcement.SetCreatedBy(user);
		announcement.SetUpdatedBy(user);
		//this.AnnouncementsCount += 1;
		return announcement;
	}
}
