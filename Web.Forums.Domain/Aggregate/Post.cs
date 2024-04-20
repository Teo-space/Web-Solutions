namespace Web.Forums.Domain.Aggregate;

public sealed partial class Post
{
	public IDType TopicId { get; set; }
	public Topic Topic { get; set; }


	public IDType PostId { get; set; }


	public string Text { get; set; }


	public bool Closed { get; set; } = false;
	public bool Deleted { get; set; } = false;


}
