namespace Web.Forums.UseCases.Forums.WriteModel;


public record CommandForumRemoveModerator(IDType ForumId, Guid UserId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumRemoveModerator>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();
			RuleFor(x => x.UserId).NotNull().NotEmpty();
		}
	}
}
