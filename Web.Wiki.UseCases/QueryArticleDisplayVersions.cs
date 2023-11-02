using Web.Wiki.Domain;

namespace Web.Wiki.UseCases;


public record QueryArticleDisplayVersions(IDType ArticleId) : IRequest<Result<IReadOnlyCollection<Article>>>
{
	public class Validator : AbstractValidator<QueryArticleDisplayVersions>
	{
		public Validator()
		{
			RuleFor(x => x.ArticleId).NotNull().NotEmpty();
		}
	}

	public class Handler(
		IValidator<QueryArticleDisplayVersions> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<QueryArticleDisplayVersions, Result<IReadOnlyCollection<Article>>>(validator)
	{
		public override async Task<Result<IReadOnlyCollection<Article>>> 
			Handle(QueryArticleDisplayVersions request, CancellationToken cancellationToken)
		{
			var Articles = await dbContext.Set<Article>()
				.Where(f => f.ArticleId == request.ArticleId)
				.ToListAsync(cancellationToken);

			if (!Articles.Any())
			{
				return Result.NotFound<IReadOnlyCollection<Article>> (request.ArticleId.ToString());
			}

			return Result.Ok(Articles as IReadOnlyCollection<Article>);
		}
	}

}


