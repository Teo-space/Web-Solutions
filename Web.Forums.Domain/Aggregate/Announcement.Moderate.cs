using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Announcement
{

	public bool CanDelete(ClaimsPrincipal principal) => CanDelete(new PrincipalUser(principal));
	public bool CanDelete(PrincipalUser user) => user.IsValid && (IsAdmin(user) || IsCurator(user));

	public Result<Announcement> Delete(ClaimsPrincipal principal, string comment)
		=> Delete(new PrincipalUser(principal), comment);
	public Result<Announcement> Delete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = true;
			//this.Moderated(user, $"MainForum.Delete", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Announcement>();
		}
	}


	public Result<Announcement> UnDelete(ClaimsPrincipal principal, string comment)
		=> UnDelete(new PrincipalUser(principal), comment);
	public Result<Announcement> UnDelete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = false;
			//this.Moderated(user, $"MainForum.UnDelete", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Announcement>();
		}
	}

}
