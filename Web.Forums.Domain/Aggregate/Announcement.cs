namespace Web.Forums.Domain.Aggregate;

public sealed partial class Announcement
{
	public IdentityType ForumId { get; set; }
	public Forum Forum { get; set; }


	public IdentityType AnnouncementId { get; set; }


	public string Title { get; set; }
	public string Text { get; set; }


	public bool Closed { get; set; } = false;
	public bool Deleted { get; set; } = false;

	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;

}
