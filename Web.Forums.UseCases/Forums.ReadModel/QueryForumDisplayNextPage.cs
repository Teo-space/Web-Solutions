namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumDisplayNextPage(IdentityType ForumId, DateTime Replied) : IRequest<Result<Forum>>
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
				.AsSplitQuery()
				.Where(f => f.ForumId == request.ForumId)
				.Include(x => x.Curators)
				.Include(x => x.Moderators)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.Include(s => s.Forums.OrderByDescending(x => x.CreatedBy.At))
				.Include(s => s.Announcements.OrderByDescending(x => x.CreatedBy.At))
				.Include(s => s.Topics.OrderByDescending(x => x.RepliedBy.At)
					.Where(x => x.RepliedBy.At < request.Replied)
					.Take(Forum.TopicsPageSize))
				.FirstOrDefaultAsync(cancellationToken);

			if (forum is null)
			{
				return Results.NotFoundById<Forum>(request.ForumId);
			}
			//if (forum.Deleted)
			//{
			//	return Results.Deleted<Forum>(request.ForumId.ToString());
			//}

			//forum.Viewed();
			//await forumDbContext.SaveChangesAsync();
			//var task = async () => await forumDbContext.SaveChangesAsync();
			//Task.Run(task);

			if (forum.IsNotRoot)
			{
				var task = forumDbContext.Forums
					.Where(f => f.ForumId == request.ForumId)
					.ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Views, f => f.Views + 1));

				task.ContinueWith(t => { }, TaskContinuationOptions.OnlyOnFaulted);
			}

			return Results.Ok(forum);
		}
	}
}


