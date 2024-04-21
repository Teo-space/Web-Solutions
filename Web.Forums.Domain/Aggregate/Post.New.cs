namespace Web.Forums.Domain.Aggregate;

public sealed partial class Post
{

	public static Post Create(Topic Topic, PrincipalUser user, string Text)
	{
		var post = new Post();
		post.Topic = Topic;
		post.TopicId = Topic.TopicId;

		post.PostId = IdentityType.NewUlid();

		post.Text = Text;

		post.SetCreatedBy(user);
		post.SetUpdatedBy(user);

		return post;
	}

}
