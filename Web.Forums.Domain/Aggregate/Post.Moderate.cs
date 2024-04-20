using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Post
{
	public bool CanDelete(PrincipalUser user)
	{
		if (user.IsValid)
		{
			return Topic.Forum.IsAdmin(user) || Topic.Forum.IsCurator(user);
		}
		return false;
	}

	public Result<Post> Delete(ClaimsPrincipal principal, string comment)
		=> Delete(new PrincipalUser(principal), comment);
	public Result<Post> Delete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = true;
			//this.Moderated(user, $"MainForum.Delete", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Post>();
		}
	}


	public Result<Post> UnDelete(ClaimsPrincipal principal, string comment)
		=> UnDelete(new PrincipalUser(principal), comment);
	public Result<Post> UnDelete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = false;
			//this.Moderated(user, $"MainForum.UnDelete", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Post>();
		}
	}
}
