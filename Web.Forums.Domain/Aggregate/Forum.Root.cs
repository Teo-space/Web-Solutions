namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{

	public static IdentityType RootId = IdentityType.Empty;
	public bool IsRoot => ForumId == RootId;
	public bool IsNotRoot => ForumId != RootId;
	public bool ParentIsRoot => ParentForumId == RootId;


	public static Forum RootForum(PrincipalUser user)
	{
		var forum = new Forum();
		forum.ForumId = RootId;


		forum.Title = "Forums";
		forum.Description = "Root Forum";

		forum.SetCreatedBy(user);
		forum.SetUpdatedBy(user);
		forum.SetRepliedBy(user);
		return forum;
	}

}
