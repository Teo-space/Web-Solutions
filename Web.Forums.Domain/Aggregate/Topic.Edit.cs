using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{
	/// <summary>
	/// Правка топика
	/// Администраторы, 
	/// Куратор, 
	/// Куратор Парента, 
	/// Автор (Если топик, форум и парент не закрыты)
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	public bool CanEdit(PrincipalUser user)
	{
		return IsAdmin(user) || IsCurator(user) || (IsAuthor(user) && !IsClosed);
	}

	public Result<Topic> Edit(ClaimsPrincipal principal, string Title, string Text)
		=> Edit(new PrincipalUser(principal), Title, Text);
	public Result<Topic> Edit(PrincipalUser user, string Title, string Text)
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
			return Results.NotEnoughPermissions<Topic>();
		}
	}
}
