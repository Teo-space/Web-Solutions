using System.Security.Claims;
using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public class Post
{
	public IDType TopicId { get; set; }
	public Topic Topic { get; set; }


	public IDType PostId { get; set; }


	public string Text { get; set; }


	public bool Closed { get; set; } = false;
	public bool Deleted { get; set; } = false;


	public UserAction CreatedBy { get; set; }
	public UserAction UpdatedBy { get; set; }

	public void SetCreatedBy(PrincipalUser user) 
		=> CreatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetCreatedBy(Guid UserId, string UserName) 
		=> CreatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetUpdatedBy(PrincipalUser user) 
		=> UpdatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetUpdatedBy(Guid UserId, string UserName) 
		=> UpdatedBy = new UserAction(UserId, UserName, DateTime.Now);


	public static Post Create(Topic Topic, PrincipalUser user, string Text)
	{
		var post = new Post();

		post.TopicId = Topic.TopicId;
		post.Topic = Topic;

		post.PostId = Ulid.NewUlid();

		post.Text = Text;

		post.SetCreatedBy(user);
		post.SetUpdatedBy(user);

		return post;
	}



	public bool CanEdit(PrincipalUser user)
	{
		if (user.IsValid)
		{
			return Topic.Forum.IsAdmin(user) || Topic.Forum.IsCurator(user);
		}
		return false;
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
