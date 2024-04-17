using System.Security.Claims;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Owned;
using Web.Forums.Domain.Permissions;

namespace Web.Forums.Domain.Aggregate;

public class Announcement
{
	public IDType ForumId { get; set; }
	public Forum Forum { get; set; }


	public IDType AnnouncementId { get; set; }


	public string Title { get; set; }
	public string Text { get; set; }


	public bool Closed { get; set; } = false;
	public bool Deleted { get; set; } = false;

	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;


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



	public static Announcement Create(Forum Forum, PrincipalUser user, string Title, string Text)
	{
		var announcement = new Announcement();
		announcement.ForumId = Forum.ForumId;
		announcement.Forum = Forum;

		announcement.AnnouncementId = Ulid.NewUlid();

		announcement.Title = Title;
		announcement.Text = Text;

		announcement.SetCreatedBy(user);
		announcement.SetUpdatedBy(user);
		//this.AnnouncementsCount += 1;
		return announcement;
	}

	public AnnouncementPermissions GetPermissions(ClaimsPrincipal principal) 
		=> new AnnouncementPermissions(this, new PrincipalUser(principal));
	public AnnouncementPermissions GetPermissions(PrincipalUser user) 
		=> new AnnouncementPermissions(this, user);


	public bool IsAdmin(PrincipalUser user) => user.IsInRole(nameof(ForumRoles.ForumRoleAdmin));
	public bool IsCurator(PrincipalUser user)
	{
		if (Forum.Curators.Any(x => x.UserId == user.UserId))
		{
			return true;
		}
		else if (Forum?.ParentForum?.Curators.Any(x => x.UserId == user.UserId) ?? false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool IsParentCurator(PrincipalUser user) 
		=> Forum?.ParentForum?.Curators.Any(x => x.UserId == user.UserId) ?? false;

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
		else if (Forum?.ParentForum?.Moderators.Any(x => x.UserId == user.UserId) ?? false)
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




	public bool CanDelete(ClaimsPrincipal principal) => CanDelete(new PrincipalUser(principal));
	public bool CanDelete(PrincipalUser user) => user.IsValid && (IsAdmin(user) || IsCurator(user));

	public Result<Announcement> Delete(ClaimsPrincipal principal, string comment)
		=> Delete(new PrincipalUser(principal), comment);
	public Result<Announcement> Delete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = true;
			//this.Moderated(user, $"MainForum.Delete", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Announcement>();
		}
	}


	public Result<Announcement> UnDelete(ClaimsPrincipal principal, string comment)
		=> UnDelete(new PrincipalUser(principal), comment);
	public Result<Announcement> UnDelete(PrincipalUser user, string comment)
	{
		if (CanDelete(user))
		{
			Deleted = false;
			//this.Moderated(user, $"MainForum.UnDelete", ForumId, comment);
			return Results.Ok(this);
		}
		else
		{
			return Results.NotEnoughPermissions<Announcement>();
		}
	}




}
