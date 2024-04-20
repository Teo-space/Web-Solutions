using System.Security.Claims;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Permissions;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Announcement
{

	public AnnouncementPermissions GetPermissions(ClaimsPrincipal principal)
		=> new AnnouncementPermissions(this, new PrincipalUser(principal));
	public AnnouncementPermissions GetPermissions(PrincipalUser user)
		=> new AnnouncementPermissions(this, user);


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
			else if (Forum?.ParentForum?.Curators.Any(x => x.UserId == user.UserId) ?? false)
			{
				return true;
			}
		}

		return false;
	}

	public bool IsParentCurator(PrincipalUser user)
	{
		return user.IsValid && (Forum?.ParentForum?.Curators.Any(x => x.UserId == user.UserId) ?? false);
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
			else if (Forum?.ParentForum?.Moderators.Any(x => x.UserId == user.UserId) ?? false)
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
