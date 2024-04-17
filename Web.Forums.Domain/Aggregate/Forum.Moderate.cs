using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{

	public bool CanDelete(PrincipalUser user)
	{
		return IsAdmin(user) || IsParentCurator(user);
	}

	public Result<Forum> Delete(ClaimsPrincipal principal, string comment)
		=> Delete(new PrincipalUser(principal), comment);
	public Result<Forum> Delete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = true;
			this.Moderated(user, $"Delete", ForumId, comment);

			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}


	public Result<Forum> UnDelete(ClaimsPrincipal principal, string comment)
		=> UnDelete(new PrincipalUser(principal), comment);
	public Result<Forum> UnDelete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = false;
			this.Moderated(user, $"UnDelete", ForumId, comment);

			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}


	public bool CanClose(PrincipalUser user)
	{
		return user.IsValid && (IsAdmin(user) || IsParentCurator(user));
	}

	public Result<Forum> Open(ClaimsPrincipal principal, string comment)
		=> Open(new PrincipalUser(principal), comment);
	public Result<Forum> Open(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = false;
			this.Moderated(user, $"Open", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}

	public Result<Forum> Close(ClaimsPrincipal principal, string comment)
		=> Close(new PrincipalUser(principal), comment);
	public Result<Forum> Close(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = true;
			this.Moderated(user, $"Close", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}


}
