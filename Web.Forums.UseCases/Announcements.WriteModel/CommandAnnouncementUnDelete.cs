namespace Web.Forums.UseCases.Announcements.WriteModel;


public record CommandAnnouncementUnDelete(IdentityType AnnouncementId, string comment) : IRequest<Result<Announcement>>
{
	public class Validator : AbstractValidator<CommandAnnouncementUnDelete>
	{
		public Validator()
		{
			RuleFor(x => x.AnnouncementId).NotNull().NotEmpty();
			RuleFor(x => x.comment).NotNull().NotEmpty().MaximumLength(100);
		}
	}


	public class Handler(IValidator<CommandAnnouncementUnDelete> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

	: AbstractHandler<CommandAnnouncementUnDelete, Result<Announcement>>(validator)
	{
		public override async Task<Result<Announcement>> Handle(CommandAnnouncementUnDelete request, CancellationToken cancellationToken)
		{
			var Announcement = await dbContext.Set<Announcement>()
				.Where(x => x.AnnouncementId == request.AnnouncementId)
				.Include(x => x.Forum)
				.Include(x => x.Forum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.Moderators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (Announcement is null)
			{
				return Results.NotFoundById<Announcement>(request.AnnouncementId);
			}

			var result = Announcement.UnDelete(userAccessor.GetUserThrowIfIsNull(), request.comment);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}

			return result;
		}
	}
}


