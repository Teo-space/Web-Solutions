namespace Web.Forums.UseCases.Forums.Curators.ReadModel;


public record QueryForumModeratorsDisplay(IdentityType ForumId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<QueryForumModeratorsDisplay>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId);
		}
	}

	public class Handler(IValidator<QueryForumModeratorsDisplay> validator, ForumDbContext forumDbContext, IUserAccessor userAccessor)

	: AbstractHandler<QueryForumModeratorsDisplay, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(QueryForumModeratorsDisplay request, CancellationToken cancellationToken)
		{
			var forum = await forumDbContext.Forums
				.AsNoTracking()
				.Where(f => f.ForumId == request.ForumId)
				.Include(x => x.Moderators)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (forum is null)
			{
				return Results.NotFoundById<Forum>(request.ForumId);
			}

			return Results.Ok(forum);
		}
	}
}
