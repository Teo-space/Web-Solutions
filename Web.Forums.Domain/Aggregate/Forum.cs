namespace Web.Forums.Domain.Aggregate;

using System.Security.Claims;
using Web.Forums.Domain;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Owned;
using Web.Forums.Domain.Permissions;

public class Forum
{
	public IDType? ParentForumId { get; set; }
	public Forum ParentForum { get; set; }

	public IDType ForumId { get; set; }




	public string Title { get; set; }
	public string Description { get; set; }

	public bool Closed { get; set; } = false;
	public bool IsClosed => Closed || (this.ParentForum != null && this.ParentForum.Closed);
	public bool Deleted { get; set; } = false;


	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;



	//public uint ForumsCount { get; set; } = 0;
	public List<Forum> Forums { get; private set; } = new List<Forum>();
	//public uint AnnouncementsCount { get; set; } = 0;
	public List<Announcement> Announcements { get; private set; } = new List<Announcement>();


	public static int TopicsPageSize { get; } = 30;
	public bool TopicsReadonly { get; private set; } = false;
	//public uint TopicsCount { get; set; } = 0;
	public List<Topic> Topics { get; private set; } = new List<Topic>();

	public ulong TopicsCount { get; private set; } = 0;
	public ulong TopicCreated() => TopicsCount++;



	public UserAction CreatedBy { get; set; }
	public UserAction UpdatedBy { get; set; }
	public UserAction RepliedBy { get; set; }

	public void SetCreatedBy(PrincipalUser user) => CreatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetCreatedBy(Guid UserId, string UserName) => CreatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetUpdatedBy(PrincipalUser user) => UpdatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetUpdatedBy(Guid UserId, string UserName) => UpdatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetRepliedBy(PrincipalUser user) => RepliedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetRepliedBy(Guid UserId, string UserName) => RepliedBy = new UserAction(UserId, UserName, DateTime.Now);



	public List<Edits> Edits { get; set; } = new List<Edits>();
	public List<Moderation> Moderations { get; set; } = new List<Moderation>();

	public Edits Edited(ClaimsPrincipal principal, string Title, string Description)
	{
		return Edited(new PrincipalUser(principal), Title, Description);
	}
	public Edits Edited(PrincipalUser User, string Title, string Description)
	{
		var e = new Edits(ForumId, Ulid.NewUlid(),
			Title, Description,
			User.UserId, User.UserName);

		Edits.Add(e);
		return e;
	}

	public Moderation Moderated(ClaimsPrincipal principal, string ActionName, IDType ObjectId, string Comment)
	{
		return Moderated(new PrincipalUser(principal), ActionName, ObjectId, Comment);
	}
	public Moderation Moderated(PrincipalUser user, string ActionName, IDType ObjectId, string Comment)
	{
		var moderatorAction = new Moderation(ForumId, Ulid.NewUlid(),
			user.UserId, user.UserName,
			ActionName, ObjectId, Comment);

		Moderations.Add(moderatorAction);
		return moderatorAction;
	}






	public List<Curator> Curators { get; set; } = new List<Curator>();
	public List<Moderator> Moderators { get; set; } = new List<Moderator>();


	public ForumPermissions GetForumPermissions(ClaimsPrincipal principal) => new ForumPermissions(this, new PrincipalUser(principal));
	public ForumPermissions GetForumPermissions(PrincipalUser user) => new ForumPermissions(this, user);


	public bool IsAdmin(PrincipalUser user) => user.IsInRole(nameof(ForumRoles.ForumRoleAdmin));
	public bool IsCurator(PrincipalUser user)
	{
		if (this.Curators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else if (this.ParentForum != null && this.ParentForum.Curators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool IsParentCurator(PrincipalUser user)
	{
		if (this.ParentForum != null && this.ParentForum.Curators.Any(x => x.UserId == user.UserId))
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
		else if (this.Moderators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else if (this.ParentForum != null && this.ParentForum.Moderators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool IsAuthor(PrincipalUser user) => CreatedBy.UserId == user.UserId;




	public static IDType RootId = IDType.Empty;
	public bool IsRoot => ForumId == RootId;
	public bool ParentIsRoot => ParentForumId == RootId;
	public static Forum RootForum()
	{
		var user = new PrincipalUser(Guid.Empty, "Root");
		var forum = new Forum();
		forum.ForumId = Ulid.Empty;


		forum.Title = "Forums";
		forum.Description = "Root Forum";

		forum.SetCreatedBy(user);
		forum.SetUpdatedBy(user);
		forum.SetRepliedBy(user);
		return forum;
	}



	public static Forum Create(Forum Parent, PrincipalUser user, string Title, string Description)
	{
		var forum = new Forum();
		forum.ParentForumId = Parent.ForumId;
		forum.ParentForum = Parent;

		forum.ForumId = Ulid.NewUlid();


		forum.Title = Title;
		forum.Description = Description;

		forum.SetCreatedBy(user);
		forum.SetUpdatedBy(user);
		forum.SetRepliedBy(user);
		forum.Edited(user, Title, Description);
		return forum;
	}


	/// <summary>
	/// Проверка прав на создание дочернего форума
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	public bool CanCreateForum(PrincipalUser user)
	{
		if (!user.IsValid)
		{
			return false;
		}
		if (Closed)
		{
			return false;
		}

		if(IsRoot || ParentIsRoot)
		{
			if (IsAdmin(user))
			{
				return true;
			}
			return false;
		}

		if (IsAdmin(user))
		{
			return true;
		}
		else if (IsCurator(user))
		{
			return true;
		}
		else
		{
			return false;
		}

	}
	public Result<Forum> CreateForum(ClaimsPrincipal principal, string Title, string Description)
		=> CreateForum(new PrincipalUser(principal), Title, Description);
	public Result<Forum> CreateForum(PrincipalUser user, string Title, string Description)
	{
		if (CanCreateForum(user))
		{
			var forum = Create(this, user, Title, Description);
			Forums.Add(forum);
			return Result.Ok(forum);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
		}
	}



	/// <summary>
	/// Проверка на право создания объявления
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	public bool CanCreateAnnouncement(PrincipalUser user)
	{
		if (!user.IsValid)
		{
			return false;
		}
		if (IsRoot || ParentIsRoot)
		{
			return false;
		}
		if (Closed)
		{
			return false;
		}

		if (IsAdmin(user))
		{
			return true;
		}
		else if (IsCurator(user))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public Result<Announcement> CreateAnnouncement(ClaimsPrincipal principal, string Title, string Text)
		=> CreateAnnouncement(new PrincipalUser(principal), Title, Text);
	public Result<Announcement> CreateAnnouncement(PrincipalUser user, string Title, string Text)
	{
		if (CanCreateAnnouncement(user))
		{
			var announcement = Announcement.Create(this, user, Title, Text);
			//this.AnnouncementsCount += 1;
			Announcements.Add(announcement);
			return Result.Ok(announcement);
		}
		else
		{
			return Result.NotEnoughPermissions<Announcement>();
		}
	}



	/// <summary>
	/// Проверка на право создания топика
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	public bool CanCreateTopic(PrincipalUser user)
	{
		if (!user.IsValid)
		{
			return false;
		}
		if (IsRoot || ParentIsRoot)
		{
			return false;
		}
		if (Closed)//Форум закрыт
		{
			return false;
		}

		if (IsAdmin(user))
		{
			return true;
		}
		else if (IsCurator(user))
		{
			return true;
		}
		else if (!TopicsReadonly)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public Result<Topic> CreateTopic(ClaimsPrincipal principal, string Title, string Text)
		=> CreateTopic(new PrincipalUser(principal), Title, Text);
	public Result<Topic> CreateTopic(PrincipalUser user, string Title, string Text)
	{
		if (CanCreateTopic(user))
		{
			var topic = Topic.Create(this, user, Title, Text);

			Topics.Add(topic);
			this.TopicCreated();

			return Result.Ok(topic);
		}
		else
		{
			return Result.NotEnoughPermissions<Topic>();
		}
	}




	public bool CanEdit(PrincipalUser user)
	{
		if (!user.IsValid)
		{
			return false;
		}

		if (IsAdmin(user))
		{
			return true;
		}
		else if (IsParentCurator(user))
		{
			return true;
		}
		else
		{
			return false;
		}
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

			return Result.Ok(this);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
		}
	}





	public bool CanDelete(PrincipalUser user)
	{
		if (!user.IsValid)
		{
			return false;
		}

		if (IsAdmin(user) || IsParentCurator(user))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public Result<Forum> Delete(ClaimsPrincipal principal, string comment)
		=> Delete(new PrincipalUser(principal), comment);
	public Result<Forum> Delete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = true;
			this.Moderated(user, $"Delete", ForumId, comment);

			return Result.Ok(this);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
		}
	}


	public Result<Forum> UnDelete(ClaimsPrincipal principal, string comment)
		=> UnDelete(new PrincipalUser(principal), comment);
	public Result<Forum> UnDelete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = false;
			this.Moderated(user, $"UnDelete", ForumId, comment);

			return Result.Ok(this);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
		}
	}





	public bool CanClose(PrincipalUser user)
	{
		if (!user.IsValid)
		{
			return false;
		}

		if (IsAdmin(user) || IsParentCurator(user))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public Result<Forum> Open(ClaimsPrincipal principal, string comment)
		=> Open(new PrincipalUser(principal), comment);
	public Result<Forum> Open(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = false;
			this.Moderated(user, $"Open", ForumId, comment);
			return Result.Ok(this);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
		}
	}

	public Result<Forum> Close(ClaimsPrincipal principal, string comment)
		=> Close(new PrincipalUser(principal), comment);
	public Result<Forum> Close(PrincipalUser user, string comment)
	{
		if (CanClose(user))
		{
			Closed = true;
			this.Moderated(user, $"Close", ForumId, comment);
			return Result.Ok(this);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
		}
	}




	public bool CanChangePrivilegedUsers(PrincipalUser user)
	{
		if (!user.IsValid)
		{
			return false;
		}
		if (IsAdmin(user))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public Result<Forum> AddCurator(ClaimsPrincipal principal, Curator curator)
		=> AddCurator(new PrincipalUser(principal), curator);
	public Result<Forum> AddCurator(PrincipalUser user, Curator curator)
	{
		if (CanChangePrivilegedUsers(user))
		{
			if (Curators.Any(x => x.UserId == curator.UserId))
			{
				return Result.Ok(this);
			}

			Curators.Add(curator);
			this.Moderated(user, $"AddCurator", ForumId, curator.UserName);

			return Result.Ok(this);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
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
				return Result.NotFound<Forum>(UserId.ToString());
			}
			Curators.RemoveAll(x => x.UserId == UserId);
			this.Moderated(user, $"RemoveCurator", ForumId, curator.UserName);

			return Result.Ok(this);
		}
		else
		{
			return Result.NotEnoughPermissions<Forum>();
		}
	}


	public Result<Moderator> AddModerator(ClaimsPrincipal principal, Moderator moderator)
		=> AddModerator(new PrincipalUser(principal), moderator);
	public Result<Moderator> AddModerator(PrincipalUser user, Moderator moderator)
	{
		if (CanChangePrivilegedUsers(user))
		{
			if (Moderators.Any(x => x.UserId == moderator.UserId))
			{
				return Result.Ok(Moderators.First(x => x.UserId == moderator.UserId));
			}
			Moderators.Add(moderator);
			this.Moderated(user, $"AddModerator", ForumId, moderator.UserName);

			return Result.Ok(moderator);
		}
		else
		{
			return Result.NotEnoughPermissions<Moderator>();
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
				return Result.NotFound<Moderator>(UserId.ToString());
			}
			Moderators.RemoveAll(x => x.UserId == moderator.UserId);
			this.Moderated(user, $"RemoveModerator", ForumId, moderator.UserName);

			return Result.Ok(moderator);
		}
		else
		{
			return Result.NotEnoughPermissions<Moderator>();
		}
	}






}
