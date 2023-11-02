using System.Security.Claims;
using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Domain.Permissions;

public record ForumPermissions
{
	Forum Forum { get; init; }
	PrincipalUser User { get; init; }


	public bool IsAdmin { get; init; }
	public bool IsCurator { get; init; }
	public bool IsParentCurator { get; init; }
	public bool IsModerator { get; init; }
	public bool IsAuthor { get; init; }


	public bool CanCreateForum { get; init; }
	public bool CanCreateAnnouncement { get; init; }
	public bool CanCreateTopic { get; init; }
	public bool CanEdit { get; init; }
	public bool CanDelete { get; init; }
	public bool CanClose { get; init; }
	public bool CanChangePrivilegedUsers { get; init; }


	public ForumPermissions(Forum Forum, ClaimsPrincipal principal) : this(Forum, new PrincipalUser(principal)) { }

	public ForumPermissions(Forum Forum, PrincipalUser user) 
	{
		this.Forum = Forum;
		this.User = user;
		{
			IsAdmin = Forum.IsAdmin(user);
			IsCurator = Forum.IsCurator(user);
			IsParentCurator = Forum.IsParentCurator(user);
			IsModerator = Forum.IsModerator(user);
			IsAuthor = Forum.IsAuthor(user);
		}
		{
			CanCreateForum = Forum.CanCreateForum(user);
			CanCreateAnnouncement = Forum.CanCreateAnnouncement(user);
			CanCreateTopic = Forum.CanCreateTopic(user);
			CanEdit = Forum.CanEdit(user);
			CanDelete = Forum.CanDelete(user);
			CanClose = Forum.CanClose(user);
			CanChangePrivilegedUsers = Forum.CanChangePrivilegedUsers(user);

		}
	}

}
