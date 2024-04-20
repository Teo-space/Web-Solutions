using System.Security.Claims;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Permissions;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{
	public TopicPermissions GetPermissions(ClaimsPrincipal principal)
		=> new TopicPermissions(this, new PrincipalUser(principal));
	public TopicPermissions GetPermissions(PrincipalUser user)
		=> new TopicPermissions(this, user);



	public bool IsAdmin(PrincipalUser user)
	{
		return user.IsValid && user.IsInRole(nameof(ForumRoles.ForumRoleAdmin));
	}

	public bool IsCurator(PrincipalUser user)
	{
		if(user.IsValid)
		{
			if (Forum.Curators.Any(x => x.UserId == user.UserId))
			{
				return true;
			}
			else if (Forum.ParentForum != null && Forum.ParentForum.Curators.Any(x => x.UserId == user.UserId))
			{
				return true;
			}
		}

		return false;
	}

	public bool IsModerator(PrincipalUser user)
	{
		if (user.IsValid)
		{
			if (user.IsInRole(nameof(ForumRoles.ForumRoleModerator)))
			{
				return true;
			}
			else if (Forum.Moderators.Any(x => x.UserId == user.UserId))
			{
				return true;
			}
			else if (Forum.ParentForum != null && Forum.ParentForum.Moderators.Any(x => x.UserId == user.UserId))
			{
				return true;
			}
		}

		return false;
	}

	public bool IsAuthor(PrincipalUser user)
	{
		return user.IsValid && CreatedBy.UserId == user.UserId;
	}

}
