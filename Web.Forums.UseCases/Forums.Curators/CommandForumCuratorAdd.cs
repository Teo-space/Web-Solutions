namespace Web.Forums.UseCases.Forums.Curators;


public record CommandForumCuratorAdd(IdentityType ForumId, Guid UserId, string UserName) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumCuratorAdd>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();
			RuleFor(x => x.UserId).NotNull().NotEmpty();
			RuleFor(x => x.UserName).NotNull().NotEmpty().MinimumLength(4).MaximumLength(50);
		}
	}
}
