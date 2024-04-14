using System.Security.Claims;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Owned;
using Web.Forums.Domain.Permissions;

namespace Web.Forums.Domain.Aggregate;

public class Topic
{
	public IDType ForumId { get; set; }
	public Forum Forum { get; set; }


	public IDType TopicId { get; set; }


	public string Title { get; set; }
	public string Text { get; set; }


	public List<Post> Posts { get; private set; } = new List<Post>();

	public ulong PostsCount { get; private set; } = 0;
	public ulong PostCreated() => PostsCount++;


	public bool Closed { get; set; } = false;
	public bool IsClosed => Closed || Forum.Closed || (Forum.ParentForum != null && Forum.ParentForum.Closed);
	public bool Deleted { get; set; } = false;


	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;


	public UserAction CreatedBy { get; private set; }
	public UserAction UpdatedBy { get; private set; }
	public UserAction RepliedBy { get; private set; }


	public void SetCreatedBy(PrincipalUser user) => CreatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetCreatedBy(Guid UserId, string UserName) => CreatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetUpdatedBy(PrincipalUser user) => UpdatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetUpdatedBy(Guid UserId, string UserName) => UpdatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetRepliedBy(PrincipalUser user) => RepliedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetRepliedBy(Guid UserId, string UserName) => RepliedBy = new UserAction(UserId, UserName, DateTime.Now);




	public static Topic Create(Forum Forum, PrincipalUser user, string Title, string Text)
	{
		var x = new Topic();
		x.ForumId = Forum.ForumId;
		x.Forum = Forum;

		x.TopicId = Ulid.NewUlid();


		x.Title = Title;
		x.Text = Text;

		x.SetCreatedBy(user);
		x.SetUpdatedBy(user);
		x.SetRepliedBy(user);
		return x;
	}


	public TopicPermissions GetPermissions(ClaimsPrincipal principal)
		=> new TopicPermissions(this, new PrincipalUser(principal));
	public TopicPermissions GetPermissions(PrincipalUser user)
		=> new TopicPermissions(this, user);




	public bool CanCreatePost(PrincipalUser user)
	{
		if (user.IsValid)
		{
			return IsAdmin(user) || IsCurator(user) || !IsClosed;
		}
		return false;
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


	public bool IsAdmin(PrincipalUser user) => user.IsInRole(nameof(ForumRoles.ForumRoleAdmin));
	public bool IsCurator(PrincipalUser user)
	{
		if (Forum.Curators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else if (Forum.ParentForum != null && Forum.ParentForum.Curators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool IsModerator(PrincipalUser user)
	{
		if (user.IsInRole(nameof(ForumRoles.ForumRoleModerator)))
		{
			return true;
		}
		else if (Forum.Moderators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else if (Forum.ParentForum != null && Forum.ParentForum.Moderators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool IsAuthor(PrincipalUser user) => CreatedBy.UserId == user.UserId;



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
		if (user.IsValid)
		{
			return IsAdmin(user) || IsCurator(user) || (IsAuthor(user) && !IsClosed);
		}
		return false;
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





	public bool CanDelete(PrincipalUser user)
	{
		if (user.IsValid)
		{
			return IsAdmin(user) || IsCurator(user);
		}
		return false;
	}

	public Result<Topic> Delete(ClaimsPrincipal principal, string comment)
		=> Delete(new PrincipalUser(principal), comment);
	public Result<Topic> Delete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = true;
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Topic>();
		}
	}


	public Result<Topic> UnDelete(ClaimsPrincipal principal, string comment)
		=> UnDelete(new PrincipalUser(principal), comment);
	public Result<Topic> UnDelete(PrincipalUser user, string comment)
	{
		if (!CanDelete(user))
		{
			return Results.NotEnoughPermissions<Topic>();
		}


		Deleted = false;
		//this.Moderated(user, $"MainForum.UnDelete", ForumId, comment);

		return Results.Ok(this);
	}





	public bool CanClose(PrincipalUser user)
	{
		if (user.IsValid)
		{
			return IsAdmin(user) || IsCurator(user);
		}
		return false;
	}

	public Result<Topic> Open(ClaimsPrincipal principal, string comment)
		=> Open(new PrincipalUser(principal), comment);
	public Result<Topic> Open(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = false;
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Topic>();
		}
	}

	public Result<Topic> Close(ClaimsPrincipal principal, string comment)
		=> Close(new PrincipalUser(principal), comment);
	public Result<Topic> Close(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = true;
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Topic>();
		}
	}









}
