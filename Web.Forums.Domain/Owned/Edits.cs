namespace Web.Forums.Domain.Owned;

public record Edits
(
	IDType OwnerId,
	IDType EditsId,


	string Title, 
	string Text, 

	Guid UserId, string UserName
);

