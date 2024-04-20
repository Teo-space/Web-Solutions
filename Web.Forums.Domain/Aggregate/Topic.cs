namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{
	public IDType ForumId { get; set; }
	public Forum Forum { get; set; }


	public IDType TopicId { get; set; }


	public string Title { get; set; }
	public string Text { get; set; }


	public List<Post> Posts { get; private set; } = new List<Post>();

	public ulong PostsCount { get; private set; } = 0;
	public ulong PostCreated() => PostsCount++;


	public bool Closed { get; set; } = false;
	public bool IsClosed => Closed || Forum.Closed || (Forum.ParentForum != null && Forum.ParentForum.Closed);
	public bool Deleted { get; set; } = false;


	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;


}
