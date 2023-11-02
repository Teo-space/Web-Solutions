namespace Web.Wiki.UseCases;
public record CommandArticleCreateVersion(Ulid ArticleId, string Title, string Description, string Text) : IRequest<Result<Article>>
{
	public class Validator : AbstractValidator<CommandArticleCreateVersion>
	{
		public Validator()
		{
			RuleFor(x => x.ArticleId).NotNull();

			RuleFor(x => x.Title)
				.NotNull().NotEmpty()
				.MinimumLength(8).MaximumLength(50)
				.Matches(@"^[A-zА-я ]+$").WithMessage("Может содержать только буквы и пробелы")
				//.Must((string Title) => !dbContext.Set<Topic>().Any(x => x.Title == Title))
				//.WithMessage($"Topic with this title already exists")
				;
			RuleFor(x => x.Text)
				.NotNull().NotEmpty()
				.MinimumLength(12).MaximumLength(100);
			RuleFor(x => x.Text)
				.NotNull().NotEmpty()
				.MinimumLength(300).MaximumLength(10000);
		}
	}

	public class Handler(
		IValidator<CommandArticleCreateVersion> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<CommandArticleCreateVersion, Result<Article>>(validator)
	{
		public override async Task<Result<Article>> Handle(CommandArticleCreateVersion request, CancellationToken cancellationToken)
		{
			var baseArticle = await dbContext.Set<Article>()
				.Where(x => x.ArticleId == request.ArticleId)
				.FirstOrDefaultAsync(cancellationToken);
			if (baseArticle == null)
			{
				return Result.NotFound<Article>(request.ArticleId.ToString());
			}
			var article = baseArticle.CreateVersion(request.Title, request.Description, request.Text);
			await dbContext.AddAsync(article);
			await dbContext.SaveChangesAsync();

			return Result.Ok(article);
		}
	}

}




