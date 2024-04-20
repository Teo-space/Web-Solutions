using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Announcement
{
	/// <summary>
	/// Правка топика
	/// Администраторы, 
	/// Куратор, 
	/// Куратор Парента, 
	/// Автор (Если топик, форум и парент не закрыты)
	/// </summary>
	public bool CanEdit(ClaimsPrincipal principal) => CanEdit(new PrincipalUser(principal));
	public bool CanEdit(PrincipalUser user) => user.IsValid ? IsAdmin(user) || IsCurator(user) : false;


	public Result<Announcement> Edit(ClaimsPrincipal principal, string Title, string Text)
		=> Edit(new PrincipalUser(principal), Title, Text);
	public Result<Announcement> Edit(PrincipalUser user, string Title, string Text)
	{
		if (CanEdit(user))
		{
			this.Title = Title;
			this.Text = Text;
			this.SetUpdatedBy(user.UserId, user.UserName);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Announcement>();
		}
	}
}
