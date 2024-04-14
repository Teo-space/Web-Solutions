namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumDisplayPage(IDType ForumId, int Offset = 0) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<QueryForumDisplayPage>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId);
			RuleFor(x => x.Offset).Must(x => x >= 0 && x <= (1000 / Forum.TopicsPageSize));
		}
	}

	public class Handler(IValidator<QueryForumDisplayPage> validator, 
		ForumDbContext forumDbContext,
		IUserAccessor userAccessor) : AbstractHandler<QueryForumDisplayPage, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(QueryForumDisplayPage request, CancellationToken cancellationToken)
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
					.Skip(request.Offset * Forum.TopicsPageSize)
					.Take(Forum.TopicsPageSize)).AsNoTracking()
				.FirstOrDefaultAsync(cancellationToken);

			if (forum is null)
			{
				return Results.NotFoundById<Forum>(request.ForumId);
			}
			//if (forum.Deleted)
			//{
			//	return Results.Deleted<Forum>(request.ForumId.ToString());
			//}

			forum.Viewed();
			await forumDbContext.SaveChangesAsync();

			return Results.Ok(forum);
		}
	}
}


