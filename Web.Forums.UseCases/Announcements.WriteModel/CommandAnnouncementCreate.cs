using Identity.UserAccessorService;

namespace Web.Forums.UseCases.Announcements.WriteModel;


public record CommandAnnouncementCreate(IDType ForumId, string Title, string Text) : IRequest<Result<Announcement>>
{
	public class Validator : AbstractValidator<CommandAnnouncementCreate>
	{
		public Validator(ForumDbContext dbContext)
		{
			RuleFor(x => x.ForumId);

			RuleFor(x => x.Title)
				.NotNull().NotEmpty()
				.MinimumLength(8).MaximumLength(50)
				.Matches(@"^[A-zА-я ]+$")
				.WithMessage("Может содержать только буквы и пробелы")
				.Must((string Title) => !dbContext.Set<Announcement>().Any(x => x.Title == Title))
				.WithMessage($"Announcement with this title already exists");

			RuleFor(x => x.Text)
				.NotNull().NotEmpty()
				.MinimumLength(50).MaximumLength(500);
		}
	}

	//userAccessor
	public class Handler(IValidator<CommandAnnouncementCreate> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

		: AbstractHandler<CommandAnnouncementCreate, Result<Announcement>>(validator)
	{
		public override async Task<Result<Announcement>> Handle(CommandAnnouncementCreate request, CancellationToken cancellationToken)
		{
			if (await dbContext.Set<Announcement>().AnyAsync(x => x.Title == request.Title, cancellationToken))
			{
				return Results.ConflictAlreadyExists<Announcement>(request.Title);
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
				return Results.ParentNotFoundById<Announcement>(request.ForumId);
			}
			var result = ParentForum.CreateAnnouncement(userAccessor.GetUserThrowIfIsNull(), request.Title, request.Text);
			if (result.Success)
			{
				await dbContext.AddAsync(result.Value);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}

