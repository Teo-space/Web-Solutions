namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{

	public static IDType RootId = IDType.Empty;
	public bool IsRoot => ForumId == RootId;
	public bool IsNotRoot => ForumId != RootId;
	public bool ParentIsRoot => ParentForumId == RootId;


	public static Forum RootForum()
	{
		var user = new PrincipalUser(Guid.Empty, "Root");
		var forum = new Forum();
		forum.ForumId = Ulid.Empty;


		forum.Title = "Forums";
		forum.Description = "Root Forum";

		forum.SetCreatedBy(user);
		forum.SetUpdatedBy(user);
		forum.SetRepliedBy(user);
		return forum;
	}

}
