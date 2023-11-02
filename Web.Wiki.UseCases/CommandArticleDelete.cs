namespace Web.Wiki.UseCases;
public record CommandArticleDelete(Ulid ArticleVersionId) : IRequest<Result<Article>>
{
	public class Validator : AbstractValidator<CommandArticleDelete>
	{
		public Validator()
		{
			RuleFor(x => x.ArticleVersionId).NotNull();
		}
	}

	public class Handler(
		IValidator<CommandArticleDelete> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<CommandArticleDelete, Result<Article>>(validator)
	{
		public override async Task<Result<Article>> Handle(CommandArticleDelete request, CancellationToken cancellationToken)
		{
			var article = await dbContext.Set<Article>()
				.Where(x => x.ArticleVersionId == request.ArticleVersionId)
				.FirstOrDefaultAsync(cancellationToken);
			if (article == null)
			{
				return Result.NotFound<Article>(request.ArticleVersionId.ToString());
			}
			article.Delete();
			await dbContext.SaveChangesAsync();

			return Result.Ok(article);
		}
	}

}




