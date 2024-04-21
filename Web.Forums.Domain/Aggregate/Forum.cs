namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	public IdentityType? ParentForumId { get; set; }
	public Forum ParentForum { get; set; }

	public IdentityType ForumId { get; set; }


	public string Title { get; set; }
	public string Description { get; set; }

	public bool Closed { get; set; } = false;
	public bool IsClosed => Closed || (this.ParentForum != null && this.ParentForum.Closed);
	public bool Deleted { get; set; } = false;

	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;

}
