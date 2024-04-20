namespace Web.Forums.Domain.Aggregate;

public sealed partial class Announcement
{
	public IDType ForumId { get; set; }
	public Forum Forum { get; set; }


	public IDType AnnouncementId { get; set; }


	public string Title { get; set; }
	public string Text { get; set; }


	public bool Closed { get; set; } = false;
	public bool Deleted { get; set; } = false;

	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;

}
