namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{

	public static Topic Create(Forum Forum, PrincipalUser user, string Title, string Text)
	{
		var topic = new Topic();
		topic.Forum = Forum;
		topic.ForumId = Forum.ForumId;

		topic.TopicId = IdentityType.NewUlid();


		topic.Title = Title;
		topic.Text = Text;

		topic.SetCreatedBy(user);
		topic.SetUpdatedBy(user);
		topic.SetRepliedBy(user);

		return topic;
	}

}
