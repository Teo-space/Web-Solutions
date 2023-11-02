namespace Web.Wiki.UseCases;
public record CommandArticleCreate(string Title, string Description, string Text) : IRequest<Result<Article>>
{
	public class Validator : AbstractValidator<CommandArticleCreate>
	{
		public Validator()
		{
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
		}
	}

	public class Handler(
		IValidator<CommandArticleCreate> validator,
		WikiDbContext dbContext,
		IHttpContextAccessor accessor
	)
		: AbstractHandler<CommandArticleCreate, Result<Article>>(validator)
	{
		public override async Task<Result<Article>> Handle(CommandArticleCreate request, CancellationToken cancellationToken)
		{
			if(await dbContext.Set<Article>().Where(x => x.Title == request.Title).AnyAsync(cancellationToken))
			{
				return Result.Conflict<Article>($"Article with this title already exists");
			}

			var article = Article.Create(request.Title, request.Description, request.Text);
			await dbContext.AddAsync(article);
			await dbContext.SaveChangesAsync();

			return Result.Ok(article);
		}
	}

}




