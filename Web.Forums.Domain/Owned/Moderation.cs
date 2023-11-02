namespace Web.Forums.Domain.Owned;

public record Moderation
(
	IDType OwnerId,
	IDType ModerationId,


	Guid ModeratedByUserId,
	string ModeratedByUserName,
	string ActionName,
	IDType ObjectId,
	string Comment

);

