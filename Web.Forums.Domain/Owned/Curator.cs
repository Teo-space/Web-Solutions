using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Domain.Owned;



public record Curator
{
	public required IdentityType CuratorId { get; init; }
	public IdentityType ForumId { get; init; }
	public Forum Forum { get; private set; }
	

	public required Guid UserId { get; init; }
	public required string UserName { get; init; }

	private Curator() { }
	public static Curator Create(Forum Forum, Guid UserId, string UserName)
		=> new Curator()
		{
			CuratorId = IdentityType.NewUlid(),
			Forum = Forum,
			UserId = UserId,
			UserName = UserName
		};

}