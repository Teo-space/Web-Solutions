using System.Security.Claims;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Permissions;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{

	public ForumPermissions GetForumPermissions(ClaimsPrincipal principal)
		=> new ForumPermissions(this, new PrincipalUser(principal));
	public ForumPermissions GetForumPermissions(PrincipalUser user)
		=> new ForumPermissions(this, user);


	public bool IsAdmin(PrincipalUser user)
	{
		return user.IsValid && user.IsInRole(nameof(ForumRoles.ForumRoleAdmin));
	}

	public bool IsCurator(PrincipalUser user)
	{
		if (user.IsValid)
		{
			if (this.Curators.Any(x => x.UserId == user.UserId))
			{
				return true;
			}
			else if (this.ParentForum != null && this.ParentForum.Curators.Any(x => x.UserId == user.UserId))
			{
				return true;
			}
		}

		return false;
	}

	public bool IsParentCurator(PrincipalUser user)
	{
		if (user.IsValid)
		{
			if (this.ParentForum != null && this.ParentForum.Curators.Any(x => x.UserId == user.UserId))
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
			else if (this.Moderators.Any(x => x.UserId == user.UserId))
			{
				return true;
			}
			else if (this.ParentForum != null && this.ParentForum.Moderators.Any(x => x.UserId == user.UserId))
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
