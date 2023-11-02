namespace Web.Forums.Domain.Owned;



public record Curator
(
	IDType ParentId,
	IDType CuratorId,
	
	Guid UserId, string UserName
);