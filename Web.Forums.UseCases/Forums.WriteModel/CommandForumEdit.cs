namespace Web.Forums.UseCases.Forums.WriteModel;



public record CommandForumEdit(IDType ForumId, string Title, string Description) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumEdit>
	{
		public Validator(ForumDbContext dbContext)
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();

			RuleFor(x => x.Title).NotNull().NotEmpty()
				.MinimumLength(8).MaximumLength(50)
				.Matches(@"^[A-zА-я ]+$")
				.WithMessage("Может содержать только буквы и пробелы");

			RuleFor(x => x.Description)
				.NotNull().NotEmpty()
				.MinimumLength(12).MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandForumEdit> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

	: AbstractHandler<CommandForumEdit, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(CommandForumEdit request, CancellationToken cancellationToken)
		{
			var Forum = await dbContext.Set<Forum>()
				.Where(x => x.ForumId == request.ForumId)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (Forum is null)
			{
				return Result.NotFoundById<Forum>(request.ForumId);
			}

			var result = Forum.Edit(userAccessor.GetUserThrowIfIsNull(), request.Title, request.Description);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}