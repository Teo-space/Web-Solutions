using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Post
{

	public bool CanEdit(PrincipalUser user)
	{
		return Topic.Forum.IsAdmin(user) || Topic.Forum.IsCurator(user);
	}

	public Result<Post> Edit(ClaimsPrincipal principal, string Text)
		=> Edit(new PrincipalUser(principal), Text);
	public Result<Post> Edit(PrincipalUser user, string Text)
	{
		if (CanEdit(user))
		{
			this.Text = Text;
			this.SetUpdatedBy(user.UserId, user.UserName);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Post>();
		}
	}

}
