using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{
	public bool CanDelete(PrincipalUser user)
	{
		return IsAdmin(user) || IsCurator(user);
	}

	public Result<Topic> Delete(ClaimsPrincipal principal, string comment)
		=> Delete(new PrincipalUser(principal), comment);
	public Result<Topic> Delete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = true;
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Topic>();
		}
	}


	public Result<Topic> UnDelete(ClaimsPrincipal principal, string comment)
		=> UnDelete(new PrincipalUser(principal), comment);
	public Result<Topic> UnDelete(PrincipalUser user, string comment)
	{
		if (!CanDelete(user))
		{
			return Results.NotEnoughPermissions<Topic>();
		}


		Deleted = false;
		//this.Moderated(user, $"MainForum.UnDelete", ForumId, comment);

		return Results.Ok(this);
	}





	public bool CanClose(PrincipalUser user)
	{
		return IsAdmin(user) || IsCurator(user);
	}

	public Result<Topic> Open(ClaimsPrincipal principal, string comment)
		=> Open(new PrincipalUser(principal), comment);
	public Result<Topic> Open(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = false;
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Topic>();
		}
	}

	public Result<Topic> Close(ClaimsPrincipal principal, string comment)
		=> Close(new PrincipalUser(principal), comment);
	public Result<Topic> Close(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = true;
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Topic>();
		}
	}

}
