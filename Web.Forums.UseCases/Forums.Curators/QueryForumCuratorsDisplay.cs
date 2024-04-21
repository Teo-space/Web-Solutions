namespace Web.Forums.UseCases.Forums.Curators.ReadModel;


public record QueryForumCuratorsDisplay(IdentityType ForumId) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<QueryForumCuratorsDisplay>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId);
		}
	}

	public class Handler(IValidator<QueryForumCuratorsDisplay> validator, ForumDbContext forumDbContext, IUserAccessor userAccessor)

	: AbstractHandler<QueryForumCuratorsDisplay, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(QueryForumCuratorsDisplay request, CancellationToken cancellationToken)
		{
			var forum = await forumDbContext.Forums
				.AsNoTracking()
				.Where(f => f.ForumId == request.ForumId)
				.Include(x => x.Curators)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.FirstOrDefaultAsync(cancellationToken);

			if (forum is null)
			{
				return Results.NotFoundById<Forum>(request.ForumId);
			}

			return Results.Ok(forum);
		}
	}
}
