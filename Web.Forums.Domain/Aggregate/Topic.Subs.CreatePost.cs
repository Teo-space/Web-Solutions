using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{
	public bool CanCreatePost(PrincipalUser user)
	{
		return IsAdmin(user) || IsCurator(user) || !IsClosed;
	}

	public Result<Post> CreatePost(ClaimsPrincipal principal, string Text)
		=> CreatePost(new PrincipalUser(principal), Text);
	public Result<Post> CreatePost(PrincipalUser user, string Text)
	{
		if (CanCreatePost(user))
		{
			var post = Post.Create(this, user, Text);
			this.Posts.Add(post);
			this.PostCreated();

			return Results.Ok(post);
		}
		else
		{
			return Results.NotEnoughPermissions<Post>();
		}
	}

}
