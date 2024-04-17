using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	//public uint AnnouncementsCount { get; set; } = 0;
	public List<Announcement> Announcements { get; private set; } = new List<Announcement>();


	/// <summary>
	/// Проверка на право создания объявления
	/// </summary>
	public bool CanCreateAnnouncement(PrincipalUser user)
	{
		return !(IsRoot || ParentIsRoot || Closed) && (IsAdmin(user) || IsCurator(user));
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
			return Results.Ok(announcement);
		}
		else
		{
			return Results.NotEnoughPermissions<Announcement>();
		}
	}

}
