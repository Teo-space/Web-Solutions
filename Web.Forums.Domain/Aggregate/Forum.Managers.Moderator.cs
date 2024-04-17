using System.Security.Claims;
using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	public List<Moderator> Moderators { get; set; } = new List<Moderator>();


	public Result<Moderator> AddModerator(ClaimsPrincipal principal, Moderator moderator)
		=> AddModerator(new PrincipalUser(principal), moderator);
	public Result<Moderator> AddModerator(PrincipalUser user, Moderator moderator)
	{
		if (CanChangePrivilegedUsers(user))
		{
			if (Moderators.Any(x => x.UserId == moderator.UserId))
			{
				return Results.Ok(Moderators.First(x => x.UserId == moderator.UserId));
			}
			Moderators.Add(moderator);
			this.Moderated(user, $"AddModerator", ForumId, moderator.UserName);

			return Results.Ok(moderator);
		}
		else
		{
			return Results.NotEnoughPermissions<Moderator>();
		}
	}


	public Result<Moderator> RemoveModerator(ClaimsPrincipal principal, Guid UserId)
		=> RemoveModerator(new PrincipalUser(principal), UserId);
	public Result<Moderator> RemoveModerator(PrincipalUser user, Guid UserId)
	{
		if (!CanChangePrivilegedUsers(user))
		{
			var moderator = Moderators.FirstOrDefault(x => x.UserId == UserId);
			if (moderator == null)
			{
				return Results.NotFound<Moderator>(UserId.ToString());
			}
			Moderators.RemoveAll(x => x.UserId == moderator.UserId);
			this.Moderated(user, $"RemoveModerator", ForumId, moderator.UserName);

			return Results.Ok(moderator);
		}
		else
		{
			return Results.NotEnoughPermissions<Moderator>();
		}
	}




}
