namespace Web.Wiki.UseCases;
public record CommandArticleApprove(Ulid ArticleVersionId) : IRequest<Result<Article>>
{
	public class Validator : AbstractValidator<CommandArticleApprove>
	{
		public Validator()
		{
			RuleFor(x => x.ArticleVersionId).NotNull();
		}
	}

	public class Handler(
		IValidator<CommandArticleApprove> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<CommandArticleApprove, Result<Article>>(validator)
	{
		public override async Task<Result<Article>> Handle(CommandArticleApprove request, CancellationToken cancellationToken)
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




