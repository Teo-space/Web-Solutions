namespace Web.Forums.UseCases.Announcements.ReadModel;


public record QueryAnnouncementGet(IdentityType AnnouncementId) : IRequest<Result<Announcement>>
{
	public class Validator : AbstractValidator<QueryAnnouncementGet>
	{
		public Validator()
		{
			RuleFor(x => x.AnnouncementId).NotNull().NotEmpty();
		}
	}

	public class Handler(
		IValidator<QueryAnnouncementGet> validator, 
		ForumDbContext forumDbContext, 
		IHttpContextAccessor accessor
	)
		: AbstractHandler<QueryAnnouncementGet, Result<Announcement>>(validator)
	{
		public override async Task<Result<Announcement>> Handle(QueryAnnouncementGet request, CancellationToken cancellationToken)
		{
			var Announcement = await forumDbContext.Set<Announcement>()
				.Where(f => f.AnnouncementId == request.AnnouncementId)
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
			return Results.Ok(Announcement);
		}
	}

}


