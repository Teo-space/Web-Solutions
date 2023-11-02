namespace Web.Forums.UseCases.Forums.WriteModel;


public record CommandForumCreate(IDType ParentForumId, string Title, string Description) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumCreate>
	{
		public Validator(ForumDbContext dbContext)
		{
			RuleFor(x => x.ParentForumId).NotNull();

			RuleFor(x => x.Title).NotNull().NotEmpty()
				.MinimumLength(8).MaximumLength(50)
				//.Matches(@"^[\w]+$").WithMessage("Может содержать только буквы и пробелы")
				.Matches(@"^[A-zА-я ]+$")
				.WithMessage("Может содержать только буквы и пробелы")
				.Must((string Title)=> !dbContext.Set<Forum>().Any(x => x.Title == Title))
				.WithMessage($"Forum with this title already exists")
				;

			RuleFor(x => x.Description)
				.NotNull().NotEmpty()
				.MinimumLength(12).MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandForumCreate> validator, ForumDbContext dbContext, IUserAccessor userAccessor) 
		
		: AbstractHandler<CommandForumCreate, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(CommandForumCreate request, CancellationToken cancellationToken)
		{
			if(await dbContext.Set<Forum>().AnyAsync(x => x.Title == request.Title, cancellationToken))
			{
				return Result.ConflictAlreadyExists<Forum>(request.Title);
			}

			var parent = await dbContext.Set<Forum>()
				.Where(x => x.ForumId == request.ParentForumId)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (parent is null)
			{
				return Result.ParentNotFoundById<Forum>(request.ParentForumId);
			}

			var result = parent.CreateForum(userAccessor.GetUserThrowIfIsNull(), request.Title, request.Description);
			if (result.Success)
			{
				await dbContext.AddAsync(result.Value);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}

