namespace Web.Forums.Domain.Aggregate;

public sealed partial class Post
{

	public static Post Create(Topic Topic, PrincipalUser user, string Text)
	{
		var post = new Post();

		post.TopicId = Topic.TopicId;
		post.Topic = Topic;

		post.PostId = Ulid.NewUlid();

		post.Text = Text;

		post.SetCreatedBy(user);
		post.SetUpdatedBy(user);

		return post;
	}

}
