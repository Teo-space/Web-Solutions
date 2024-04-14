namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumGet(IDType ForumId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<QueryForumGet>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId);
		}
	}

	public class Handler(IValidator<QueryForumGet> validator, ForumDbContext forumDbContext, IUserAccessor userAccessor)

		: AbstractHandler<QueryForumGet, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(QueryForumGet request, CancellationToken cancellationToken)
		{
			var forum = await forumDbContext.Forums.AsNoTracking()
				.Where(f => f.ForumId == request.ForumId)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (forum is null)
			{
				return Results.NotFoundById<Forum>(request.ForumId);
			}
			if (forum.Deleted)
			{
				return Results.Deleted<Forum>(request.ForumId.ToString());
			}

			return Results.Ok(forum);
		}
	}
}
