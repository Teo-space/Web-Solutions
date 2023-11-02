using System.Security.Claims;
using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Domain.Permissions;

public record TopicPermissions
{
	Topic Topic { get; init; }
	PrincipalUser User { get; init; }


	public bool IsAdmin { get; init; }
	public bool IsCurator { get; init; }
	public bool IsModerator { get; init; }
	public bool IsAuthor { get; init; }


	public bool CanEdit { get; init; }
	public bool CanDelete { get; init; }
	public bool CanClose { get; init; }


	public TopicPermissions(Topic Topic, ClaimsPrincipal principal) : this(Topic, new PrincipalUser(principal)) { }

	public TopicPermissions(Topic Topic, PrincipalUser user) 
	{
		this.Topic = Topic;
		this.User = user;
		{
			IsAdmin = Topic.IsAdmin(user);
			IsCurator = Topic.IsCurator(user);
			IsModerator = Topic.IsModerator(user);
			IsAuthor = Topic.IsAuthor(user);
		}
		{
			CanEdit = Topic.CanEdit(user);
			CanDelete = Topic.CanDelete(user);
			CanClose = Topic.CanClose(user);
		}
	}

}
