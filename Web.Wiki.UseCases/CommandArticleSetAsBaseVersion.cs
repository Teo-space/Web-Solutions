namespace Web.Wiki.UseCases;
public record CommandArticleSetAsBaseVersion(Ulid ArticleVersionId) : IRequest<Result<Article>>
{
	public class Validator : AbstractValidator<CommandArticleSetAsBaseVersion>
	{
		public Validator()
		{
			RuleFor(x => x.ArticleVersionId).NotNull();
		}
	}

	public class Handler(
		IValidator<CommandArticleSetAsBaseVersion> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<CommandArticleSetAsBaseVersion, Result<Article>>(validator)
	{
		public override async Task<Result<Article>> Handle(CommandArticleSetAsBaseVersion request, CancellationToken cancellationToken)
		{
			var article = await dbContext.Set<Article>()
				.Where(x => x.ArticleVersionId == request.ArticleVersionId)
				.FirstOrDefaultAsync(cancellationToken);
			if (article == null)
			{
				return Result.NotFound<Article>(request.ArticleVersionId.ToString());
			}
			var baseArticle = await dbContext.Set<Article>()
				.Where(x => x.ArticleId == article.ArticleId)
				.Include(x => x.ArticleVersions)
				.FirstOrDefaultAsync(cancellationToken);

			foreach(var version in baseArticle.ArticleVersions.Where(x => x.ArticleVersionId != article.ArticleVersionId))
			{
				//Перенести этот код в саму модель
			}

			article.SetAsBaseVersion();
			await dbContext.SaveChangesAsync();

			return Result.Ok(article);
		}
	}

}




