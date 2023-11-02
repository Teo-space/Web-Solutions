using System.Security.Claims;
using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Domain.Permissions;

public record AnnouncementPermissions
{
	Announcement Announcement { get; init; }
	PrincipalUser User { get; init; }

	public bool IsAdmin { get; init; }
	public bool IsCurator { get; init; }
	public bool IsParentCurator { get; init; }
	public bool IsModerator { get; init; }
	public bool IsAuthor { get; init; }



	public bool CanEdit { get; init; }
	public bool CanDelete { get; init; }



	public AnnouncementPermissions(Announcement Announcement, ClaimsPrincipal principal) 
		: this(Announcement, new PrincipalUser(principal)) { }

	public AnnouncementPermissions(Announcement Announcement, PrincipalUser user) 
	{
		this.Announcement = Announcement;
		this.User = user;
		{
			IsAdmin = Announcement.IsAdmin(user);
			IsCurator = Announcement.IsCurator(user);
			IsParentCurator = Announcement.IsParentCurator(user);
			IsModerator = Announcement.IsModerator(user);
			IsAuthor = Announcement.IsAuthor(user);
		}
		{
			CanEdit = Announcement.CanEdit(user);
			CanDelete = Announcement.CanDelete(user);
		}
	}

}
