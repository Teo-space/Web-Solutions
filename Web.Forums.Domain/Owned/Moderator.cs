namespace Web.Forums.Domain.Owned;



public record Moderator
(
	IDType ParentId,
	IDType ModeratorId, 

	Guid UserId, string UserName
);