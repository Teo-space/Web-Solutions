using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	//public uint ForumsCount { get; set; } = 0;
	public List<Forum> Forums { get; private set; } = new List<Forum>();


	/// <summary>
	/// Проверка прав на создание дочернего форума
	/// </summary>
	public bool CanCreateForum(PrincipalUser user)
	{
		return IsAdmin(user) || (!Closed && !(IsRoot || ParentIsRoot) && IsCurator(user));
	}

	public Result<Forum> CreateForum(ClaimsPrincipal principal, string Title, string Description)
		=> CreateForum(new PrincipalUser(principal), Title, Description);
	public Result<Forum> CreateForum(PrincipalUser user, string Title, string Description)
	{
		if (CanCreateForum(user))
		{
			var forum = Forum.Create(this, user, Title, Description);
			Forums.Add(forum);

			return Results.Ok(forum);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}


	public static Forum Create(Forum Parent, PrincipalUser user, string Title, string Description)
	{
		var forum = new Forum();
		//forum.ParentForum = Parent;
		//forum.ParentForumId = Parent.ForumId;

		forum.ForumId = IdentityType.NewUlid();


		forum.Title = Title;
		forum.Description = Description;

		forum.SetCreatedBy(user);
		forum.SetUpdatedBy(user);
		forum.SetRepliedBy(user);
		forum.Edited(user, Title, Description);
		return forum;
	}
}

