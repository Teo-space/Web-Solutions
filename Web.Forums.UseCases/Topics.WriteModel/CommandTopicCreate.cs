namespace Web.Forums.UseCases.Topics.WriteModel;


public record CommandTopicCreate(IDType ForumId, string Title, string Text) : IRequest<Result<Topic>>
{
	public class Validator : AbstractValidator<CommandTopicCreate>
	{
		public Validator(ForumDbContext dbContext)
		{
			RuleFor(x => x.ForumId);
			RuleFor(x => x.Title)
				.NotNull().NotEmpty()
				.MinimumLength(8).MaximumLength(50)
				.Matches(@"^[A-zА-я ]+$")
				.WithMessage("Может содержать только буквы и пробелы")
				.Must((string Title) => !dbContext.Set<Topic>().Any(x => x.Title == Title))
				.WithMessage($"Topic with this title already exists");
			RuleFor(x => x.Text)
				.NotNull().NotEmpty()
				.MinimumLength(12).MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandTopicCreate> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

	: AbstractHandler<CommandTopicCreate, Result<Topic>>(validator)
	{
		public override async Task<Result<Topic>> Handle(CommandTopicCreate request, CancellationToken cancellationToken)
		{
			if (await dbContext.Set<Announcement>().AnyAsync(x => x.Title == request.Title, cancellationToken))
			{
				return Result.ConflictAlreadyExists<Topic>(request.Title);
			}

			var ParentForum = await dbContext.Set<Forum>()
				.Where(x => x.ForumId == request.ForumId)
				.Include(x => x.Curators)
				.Include(x => x.Moderators)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (ParentForum is null)
			{
				return Result.NotFoundById<Topic>(request.ForumId);
			}
			var result = ParentForum.CreateTopic(userAccessor.GetUserThrowIfIsNull(), request.Title, request.Text);
			if (result.Success)
			{
				await dbContext.AddAsync(result.Value);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
