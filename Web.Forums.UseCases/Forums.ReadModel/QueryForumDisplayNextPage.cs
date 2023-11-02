namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumDisplayNextPage(IDType ForumId, DateTime Replied) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<QueryForumDisplayNextPage>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId);
			RuleFor(x => x.Replied).NotNull().NotEmpty();
		}
	}

	public class Handler(IValidator<QueryForumDisplayNextPage> validator, 
		ForumDbContext forumDbContext,
		IUserAccessor userAccessor) : AbstractHandler<QueryForumDisplayNextPage, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(QueryForumDisplayNextPage request, CancellationToken cancellationToken)
		{
			var forum = await forumDbContext.Forums
				.AsNoTracking()
				.Where(f => f.ForumId == request.ForumId)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.Include(s => s.Forums).AsNoTracking()
				.Include(s => s.Announcements).AsNoTracking()
				.Include(s => s.Topics
					.OrderByDescending(x => x.RepliedBy.At)
					.Where(x => x.RepliedBy.At < request.Replied)
					.Take(Forum.TopicsPageSize))
				.AsNoTracking()
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


