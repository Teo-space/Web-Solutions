namespace Web.Forums.Domain.Owned;



public record Curator
(
	IDType ParentId,
	IDType CuratorId,
	
	Guid UserId, string UserName
)
{
	public static Curator Create(IDType ParentId, Guid UserId, string UserName) 
		=> new Curator(ParentId, Ulid.NewUlid(), UserId, UserName);

}