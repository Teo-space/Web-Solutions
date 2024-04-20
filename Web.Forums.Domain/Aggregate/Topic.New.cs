namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{

	public static Topic Create(Forum Forum, PrincipalUser user, string Title, string Text)
	{
		var x = new Topic();
		x.ForumId = Forum.ForumId;
		x.Forum = Forum;

		x.TopicId = Ulid.NewUlid();


		x.Title = Title;
		x.Text = Text;

		x.SetCreatedBy(user);
		x.SetUpdatedBy(user);
		x.SetRepliedBy(user);
		return x;
	}

}
