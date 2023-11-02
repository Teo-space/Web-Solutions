namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumDisplay(IDType ForumId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<QueryForumDisplay>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId);
		}
	}

	//IHttpContextAccessor accessor
	public class Handler(IValidator<QueryForumDisplay> validator, ForumDbContext forumDbContext, IUserAccessor userAccessor)

	: AbstractHandler<QueryForumDisplay, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(QueryForumDisplay request, CancellationToken cancellationToken)
		{
			var forum = await forumDbContext.Forums.AsNoTracking()
				.Where(f => f.ForumId == request.ForumId)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.Include(s => s.Forums.OrderBy(x => x.CreatedBy.At)).AsNoTracking()
				.Include(s => s.Announcements.OrderBy(x => x.CreatedBy.At)).AsNoTracking()
				.Include(s => s.Topics
					.OrderByDescending(x => x.RepliedBy.At)
					.Take(Forum.TopicsPageSize)).AsNoTracking()
				.FirstOrDefaultAsync(cancellationToken);

			if (forum is null)
			{
				return Result.NotFoundById<Forum>(request.ForumId);
			}
			//if (forum.Deleted)
			//{
			//	return Results.Deleted<Forum>(request.ForumId.ToString());
			//}

			forum.Viewed();
			await forumDbContext.SaveChangesAsync();

			return Result.Ok(forum);
		}
	}
}
