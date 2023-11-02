using Web.Wiki.Domain;

namespace Web.Wiki.UseCases;


public record QueryArticleDisplayAwaitingApproving(IDType ArticleId) 
	
	: IRequest<Result<IReadOnlyCollection<Article>>>
{
	public class Validator : AbstractValidator<QueryArticleDisplayAwaitingApproving>
	{
		public Validator()
		{
			RuleFor(x => x.ArticleId).NotNull().NotEmpty();
		}
	}

	public class Handler(
		IValidator<QueryArticleDisplayAwaitingApproving> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<QueryArticleDisplayAwaitingApproving, Result<IReadOnlyCollection<Article>>>(validator)
	{
		public override async Task<Result<IReadOnlyCollection<Article>>> 
			Handle(QueryArticleDisplayAwaitingApproving request, CancellationToken cancellationToken)
		{
			var Articles = await dbContext.Set<Article>()
				.Where(x => !x.Approved)
				.ToListAsync(cancellationToken);

			return Result.Ok(Articles as IReadOnlyCollection<Article>);
		}
	}

}
