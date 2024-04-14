namespace Web.Forums.UseCases.Topics.ReadModel;


public record QueryTopicDisplay(IDType TopicId) : IRequest<Result<Topic>>
{
	public class Validator : AbstractValidator<QueryTopicDisplay>
	{
		public Validator()
		{
			RuleFor(x => x.TopicId).NotNull().NotEmpty();
		}
	}

	public class Handler(
			IValidator<QueryTopicDisplay> validator,
			ForumDbContext forumDbContext,
			IUserAccessor userAccessor)

		: AbstractHandler<QueryTopicDisplay, Result<Topic>>(validator)
	{
		public override async Task<Result<Topic>> Handle(QueryTopicDisplay request, CancellationToken cancellationToken)
		{
			var Topic = await forumDbContext.Set<Topic>()
				.Where(f => f.TopicId == request.TopicId)
				.Include(x => x.Forum)
				.Include(x => x.Forum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.Moderators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.Include(x => x.Posts)
				.FirstOrDefaultAsync(cancellationToken);

			if (Topic is null)
			{
				return Results.NotFoundById<Topic>(request.TopicId);
			}

			Topic.Viewed();
			await forumDbContext.SaveChangesAsync();

			return Results.Ok(Topic);
		}
	}
}
