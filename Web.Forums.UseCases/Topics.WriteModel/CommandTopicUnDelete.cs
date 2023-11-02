namespace Web.Forums.UseCases.Topics.WriteModel;


public record CommandTopicUnDelete(IDType TopicId, string comment) : IRequest<Result<Topic>>
{
	public class Validator : AbstractValidator<CommandTopicUnDelete>
	{
		public Validator()
		{
			RuleFor(x => x.TopicId).NotNull().NotEmpty();
			RuleFor(x => x.comment).NotNull().NotEmpty().MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandTopicUnDelete> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

	: AbstractHandler<CommandTopicUnDelete, Result<Topic>>(validator)
	{
		public override async Task<Result<Topic>> Handle(CommandTopicUnDelete request, CancellationToken cancellationToken)
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
				return Result.NotFoundById<Topic>(request.TopicId);
			}
			var result = Topic.UnDelete(userAccessor.GetUserThrowIfIsNull(), request.comment);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
