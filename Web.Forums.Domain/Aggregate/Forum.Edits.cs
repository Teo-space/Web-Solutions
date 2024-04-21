using System.Security.Claims;
using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	public List<Edits> Edits { get; set; } = new List<Edits>();


	public Edits Edited(ClaimsPrincipal principal, string Title, string Description)
	{
		return Edited(new PrincipalUser(principal), Title, Description);
	}

	public Edits Edited(PrincipalUser User, string Title, string Description)
	{
		//ForumId, 
		var e = Owned.Edits.Create(this, Title, Description, User.UserId, User.UserName);

		Edits.Add(e);
		return e;
	}

}
