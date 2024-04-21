using System.Security.Claims;
using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	public List<Curator> Curators { get; set; } = new List<Curator>();


	public bool CanChangePrivilegedUsers(PrincipalUser user) => IsAdmin(user);


	public Result<Forum> AddCurator(ClaimsPrincipal principal, Curator curator)
		=> AddCurator(new PrincipalUser(principal), curator);
	public Result<Forum> AddCurator(PrincipalUser user, Curator curator)
	{
		if (CanChangePrivilegedUsers(user))
		{
			if (Curators.Any(x => x.UserId == curator.UserId))
			{
				return Results.Ok(this);
			}

			Curators.Add(curator);

			this.Moderated(user, $"AddCurator", ForumId, curator.UserName);

			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}


	public Result<Forum> RemoveCurator(ClaimsPrincipal principal, Guid UserId)
		=> RemoveCurator(new PrincipalUser(principal), UserId);
	public Result<Forum> RemoveCurator(PrincipalUser user, Guid UserId)
	{
		if (CanChangePrivilegedUsers(user))
		{
			var curator = Curators.FirstOrDefault(x => x.UserId == UserId);
			if (curator == null)
			{
				return Results.NotFound<Forum>(UserId.ToString());
			}
			Curators.RemoveAll(x => x.UserId == UserId);
			this.Moderated(user, $"RemoveCurator", ForumId, curator.UserName);

			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Forum>();
		}
	}




}
