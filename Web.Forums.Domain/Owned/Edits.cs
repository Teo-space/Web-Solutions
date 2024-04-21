using Web.Forums.Domain.Aggregate;

namespace Web.Forums.Domain.Owned;

public record Edits
{

	public IdentityType ForumId {  get; init; }
	public Forum Forum {  get; set; }

	public required IdentityType EditsId { get; init; }

	public required string Title {  get; init; }
	public required string Text {  get; init; }

	public required Guid UserId {  get; init; }
	public required string UserName {  get; init; }


	private Edits() { }
	public static Edits Create(Forum Forum,
		string Title, string Text,
		Guid UserId, string UserName)
	{
		return new Edits
		{
			Forum = Forum,
			EditsId = IdentityType.NewUlid(),
			Title = Title,
			Text = Text,
			UserId = UserId,
			UserName = UserName
		};
	}
}

