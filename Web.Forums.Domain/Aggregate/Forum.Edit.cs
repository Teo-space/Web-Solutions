using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	public bool CanEdit(PrincipalUser user)
	{
		return IsAdmin(user) || IsParentCurator(user);
	}

	public Result<Forum> Edit(ClaimsPrincipal principal, string Title, string Description)
		=> Edit(new PrincipalUser(principal), Title, Description);
	public Result<Forum> Edit(PrincipalUser user, string Title, string Description)
	{
		if (CanEdit(user))
		{
			this.Title = Title;
			this.Description = Description;

			this.SetUpdatedBy(user.UserId, user.UserName);

			this.Edited(user, Title, Description);

			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}
}
