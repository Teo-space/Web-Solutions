namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumExistsByTitle(string Title) : IRequest<bool>
{
	public class Validator : AbstractValidator<QueryForumExistsByTitle>
	{
		public Validator()
		{
			RuleFor(x => x.Title).NotNull().NotEmpty().MinimumLength(8).MaximumLength(50);
		}
	}

	public class Handler(IValidator<QueryForumExistsByTitle> validator, ForumDbContext forumDbContext, IUserAccessor userAccessor)

		: AbstractHandler<QueryForumExistsByTitle, bool>(validator)
	{
		public override async Task<bool> Handle(QueryForumExistsByTitle request, CancellationToken cancellationToken)
		{
			return await forumDbContext.Forums
				.AsNoTracking()
				.Where(f => f.Title == request.Title)
				.AnyAsync(cancellationToken);
		}
	}
}
