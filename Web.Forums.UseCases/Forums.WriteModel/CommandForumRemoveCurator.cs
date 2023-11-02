namespace Web.Forums.UseCases.Forums.WriteModel;


public record CommandForumRemoveCurator(IDType ForumId, Guid UserId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumRemoveCurator>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();
			RuleFor(x => x.UserId).NotNull().NotEmpty();
		}
	}
}
