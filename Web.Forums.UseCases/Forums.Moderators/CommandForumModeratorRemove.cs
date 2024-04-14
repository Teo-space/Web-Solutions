namespace Web.Forums.UseCases.Forums.Moderators;


public record CommandForumModeratorRemove(IDType ForumId, Guid UserId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumModeratorRemove>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();
			RuleFor(x => x.UserId).NotNull().NotEmpty();
		}
	}
}
