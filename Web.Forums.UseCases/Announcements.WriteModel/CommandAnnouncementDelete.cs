namespace Web.Forums.UseCases.Announcements.WriteModel;


public record CommandAnnouncementDelete(IDType AnnouncementId, string comment) : IRequest<Result<Announcement>>
{
	public class Validator : AbstractValidator<CommandAnnouncementDelete>
	{
		public Validator()
		{
			RuleFor(x => x.AnnouncementId).NotNull().NotEmpty();
			RuleFor(x => x.comment).NotNull().NotEmpty().MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandAnnouncementDelete> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

		: AbstractHandler<CommandAnnouncementDelete, Result<Announcement>>(validator)
	{
		public override async Task<Result<Announcement>> Handle(CommandAnnouncementDelete request, CancellationToken cancellationToken)
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
				return Result.NotFoundById<Announcement>(request.AnnouncementId);
			}
			var result = Announcement.Delete(userAccessor.GetUserThrowIfIsNull(), request.comment);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
