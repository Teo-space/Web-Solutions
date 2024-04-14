namespace Web.Forums.UseCases.Announcements.WriteModel;


public record CommandAnnouncementEdit(IDType AnnouncementId, string Title, string Text) : IRequest<Result<Announcement>>
{
	public class Validator : AbstractValidator<CommandAnnouncementEdit>
	{
		public Validator(ForumDbContext dbContext)
		{
			RuleFor(x => x.AnnouncementId);
			RuleFor(x => x.Title)
				.NotNull().NotEmpty()
				.MinimumLength(8).MaximumLength(50)
				.Matches(@"^[A-zА-я ]+$")
				.WithMessage("Может содержать только буквы и пробелы");
			RuleFor(x => x.Text)
				.NotNull().NotEmpty()
				.MinimumLength(12).MaximumLength(500);
		}
	}

	public class Handler(IValidator<CommandAnnouncementEdit> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

	: AbstractHandler<CommandAnnouncementEdit, Result<Announcement>>(validator)
	{
		public override async Task<Result<Announcement>> Handle(CommandAnnouncementEdit request, CancellationToken cancellationToken)
		{
			var Announcement = await dbContext.Set<Announcement>()
				.Where(x => x.AnnouncementId == request.AnnouncementId)
				.Include(x => x.Forum)
				.Include(x => x.Forum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.Moderators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken)
				;
			if (Announcement is null)
			{
				return Results.NotFoundById<Announcement>(request.AnnouncementId);
			}
			var result = Announcement.Edit(userAccessor.GetUserThrowIfIsNull(), request.Title, request.Text);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
