namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumDisplayPreviousPage(IDType ForumId, DateTime Replied) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<QueryForumDisplayPreviousPage>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId);
			RuleFor(x => x.Replied).NotNull();
		}
	}

	public class Handler(
		IValidator<QueryForumDisplayPreviousPage> validator, 
		ForumDbContext forumDbContext, 
		IUserAccessor userAccessor)

	: AbstractHandler<QueryForumDisplayPreviousPage, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(QueryForumDisplayPreviousPage request, CancellationToken cancellationToken)
		{
			var forum = await forumDbContext.Forums
				.AsNoTracking()
				.Where(f => f.ForumId == request.ForumId)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.Include(s => s.Forums)
				.Include(s => s.Announcements)
				.Include(s => s.Topics
					.OrderBy(x => x.RepliedBy.At)
					.Where(x => x.RepliedBy.At >= request.Replied)
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
			forum.Topics.Sort((a, b) => b.RepliedBy.At.CompareTo(a.RepliedBy.At));

			//forum.Viewed();
			//await forumDbContext.SaveChangesAsync();
			//var task = async () => await forumDbContext.SaveChangesAsync();
			//.Run(task);

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
