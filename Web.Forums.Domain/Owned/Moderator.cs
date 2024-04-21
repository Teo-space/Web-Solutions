using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Domain.Owned;



public record Moderator
{
	public IdentityType ForumId { get; init; }
	public Forum Forum { get; init; }

	public required IdentityType ModeratorId { get; init; }


	public required Guid UserId { get; init; }
	public required string UserName { get; init; }

	private Moderator() { }
	public static Moderator Create(Forum Forum, Guid UserId, string UserName)
		=> new Moderator()
		{
			Forum = Forum,
			ModeratorId = IdentityType.NewUlid(),
			UserId = UserId,
			UserName = UserName
		};
}