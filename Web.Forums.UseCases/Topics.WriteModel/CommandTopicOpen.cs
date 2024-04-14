namespace Web.Forums.UseCases.Topics.WriteModel;


public record CommandTopicOpen(IDType TopicId, string comment) : IRequest<Result<Topic>>
{
	public class Validator : AbstractValidator<CommandTopicOpen>
	{
		public Validator()
		{
			RuleFor(x => x.TopicId).NotNull().NotEmpty();
			RuleFor(x => x.comment).NotNull().NotEmpty().MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandTopicOpen> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

	: AbstractHandler<CommandTopicOpen, Result<Topic>>(validator)
	{
		public override async Task<Result<Topic>> Handle(CommandTopicOpen request, CancellationToken cancellationToken)
		{
			var Topic = await dbContext.Set<Topic>()
				.Where(x => x.TopicId == request.TopicId)
				.Include(x => x.Forum)
				.Include(x => x.Forum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.Moderators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.Forum).ThenInclude(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (Topic is null)
			{
				return Results.NotFoundById<Topic>(request.TopicId);
			}
			var result = Topic.Open(userAccessor.GetUserThrowIfIsNull(), request.comment);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
