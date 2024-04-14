namespace Web.Forums.UseCases.Forums.Curators;


public record CommandForumCuratorRemove(IDType ForumId, Guid UserId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumCuratorRemove>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();
			RuleFor(x => x.UserId).NotNull().NotEmpty();
		}
	}
}
