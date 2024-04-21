namespace Web.Forums.UseCases.Posts.WriteModel;


public record CommandPostCreate(IdentityType TopicId, string Text) : IRequest<Result<Post>>
{
	public class Validator : AbstractValidator<CommandPostCreate>
	{
		public Validator(ForumDbContext dbContext)
		{
			RuleFor(x => x.TopicId);

			RuleFor(x => x.Text).NotNull().NotEmpty().MinimumLength(12).MaximumLength(500);
		}
	}

	public class Handler(IValidator<CommandPostCreate> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

		: AbstractHandler<CommandPostCreate, Result<Post>>(validator)
	{
		public override async Task<Result<Post>> Handle(CommandPostCreate request, CancellationToken cancellationToken)
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
				return Results.ParentNotFoundById<Post>(request.TopicId);
			}

			var result = Topic.CreatePost(userAccessor.GetUserThrowIfIsNull(), request.Text);
			if (result.Success)
			{
				await dbContext.AddAsync(result.Value);
				await dbContext.SaveChangesAsync();
			}

			return result;
		}
	}
}
