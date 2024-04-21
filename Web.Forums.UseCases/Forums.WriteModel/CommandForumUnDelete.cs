namespace Web.Forums.UseCases.Forums.WriteModel;


public record CommandForumUnDelete(IdentityType ForumId, string comment) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumUnDelete>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();
			RuleFor(x => x.comment).NotNull().NotEmpty().MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandForumUnDelete> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

		: AbstractHandler<CommandForumUnDelete, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(CommandForumUnDelete request, CancellationToken cancellationToken)
		{
			var forum = await dbContext.Set<Forum>()
				.Where(x => x.ForumId == request.ForumId)
				.Include(x => x.Curators)
				.Include(x => x.Moderators)
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(cancellationToken);

			if (forum is null)
			{
				return Results.NotFoundById<Forum>(request.ForumId);
			}

			var result = forum.UnDelete(userAccessor.GetUserThrowIfIsNull(), request.comment);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}

			return result;
		}
	}

}
