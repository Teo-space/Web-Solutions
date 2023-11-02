using Web.Wiki.Domain;

namespace Web.Wiki.UseCases;


public record QueryArticleDisplay(IDType ArticleId) : IRequest<Result<Article>>
{
	public class Validator : AbstractValidator<QueryArticleDisplay>
	{
		public Validator()
		{
			RuleFor(x => x.ArticleId).NotNull().NotEmpty();
		}
	}

	public class Handler(
		IValidator<QueryArticleDisplay> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<QueryArticleDisplay, Result<Article>>(validator)
	{
		public override async Task<Result<Article>> Handle(QueryArticleDisplay request, CancellationToken cancellationToken)
		{
			var Article = await dbContext.Set<Article>()
				.Where(x => x.ArticleId == request.ArticleId)
				.Where(x => x.BaseVersion)
				.FirstOrDefaultAsync(cancellationToken);

			if (Article == null)
			{
				return Result.NotFound<Article>(request.ArticleId.ToString());
			}
			Article.Viewed();
			await dbContext.SaveChangesAsync();

			return Result.Ok(Article);
		}
	}

}


