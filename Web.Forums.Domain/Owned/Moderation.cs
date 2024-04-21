using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Domain.Owned;

public record Moderation
{
	public IdentityType ForumId { get; init; }
	public Forum Forum { get; init; }
	public required IdentityType ModerationId { get; init; }

	public required Guid UserId { get; init; }
	public required string UserName { get; init; }
	public required string ActionName { get; init; }
	public required IdentityType ObjectId { get; init; }
	public required string Comment { get; init; }

	private Moderation() { }
	public static Moderation Create(Forum Forum,
		Guid UserId, string UserName,
		string ActionName, IdentityType ObjectId, string Comment)
	{
		return new Moderation
		{
			Forum = Forum,
			ModerationId = IdentityType.NewUlid(),
			UserId = UserId,
			UserName = UserName,
			ActionName = ActionName,
			ObjectId = ObjectId,
			Comment = Comment
		};
	}

}

