namespace Web.Forums.UseCases.Topics.WriteModel;


public record CommandTopicEdit(IdentityType TopicId, string Title, string Text) : IRequest<Result<Topic>>
{
	public class Validator : AbstractValidator<CommandTopicEdit>
	{
		public Validator()
		{
			RuleFor(x => x.TopicId);
			RuleFor(x => x.Title)
				.NotNull().NotEmpty()
				.MinimumLength(8).MaximumLength(50)
				.Matches(@"^[A-zА-я ]+$").WithMessage("Может содержать только буквы и пробелы")
				;

			RuleFor(x => x.Text)
				.NotNull().NotEmpty()
				.MinimumLength(12).MaximumLength(100)
				;
		}
	}


	public class Handler(IValidator<CommandTopicEdit> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

		: AbstractHandler<CommandTopicEdit, Result<Topic>>(validator)
	{
		public override async Task<Result<Topic>> Handle(CommandTopicEdit request, CancellationToken cancellationToken)
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

			var result = Topic.Edit(userAccessor.GetUserThrowIfIsNull(), request.Title, request.Text);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}

			return result;
		}
	}

}
