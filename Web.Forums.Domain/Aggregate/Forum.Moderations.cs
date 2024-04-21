using System.Security.Claims;
using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	public List<Moderation> Moderations { get; set; } = new List<Moderation>();

	public Moderation Moderated(ClaimsPrincipal principal, string ActionName, IdentityType ObjectId, string Comment)
	{
		return Moderated(new PrincipalUser(principal), ActionName, ObjectId, Comment);
	}
	public Moderation Moderated(PrincipalUser user, string ActionName, IdentityType ObjectId, string Comment)
	{
		var moderatorAction = Moderation.Create(this, user.UserId, user.UserName, ActionName, ObjectId, Comment);
		Moderations.Add(moderatorAction);

		return moderatorAction;
	}
}
