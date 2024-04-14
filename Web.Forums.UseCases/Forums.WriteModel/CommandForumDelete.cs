namespace Web.Forums.UseCases.Forums.WriteModel;


public record CommandForumDelete(IDType ForumId, string comment) : IRequest<Result<Forum>>
{
	public class Validator : AbstractValidator<CommandForumDelete>
	{
		public Validator()
		{
			RuleFor(x => x.ForumId).NotNull().NotEmpty();
			RuleFor(x => x.comment).NotNull().NotEmpty().MaximumLength(100);
		}
	}

	public class Handler(IValidator<CommandForumDelete> validator, ForumDbContext dbContext, IUserAccessor userAccessor)

		: AbstractHandler<CommandForumDelete, Result<Forum>>(validator)
	{
		public override async Task<Result<Forum>> Handle(CommandForumDelete request, CancellationToken cancellationToken)
		{
			var forum = await dbContext.Set<Forum>()
				.Include(x => x.ParentForum)
				.Include(x => x.ParentForum).ThenInclude(x => x.Curators)
				.Include(x => x.ParentForum).ThenInclude(x => x.Moderators)
				.FirstOrDefaultAsync(x => x.ForumId == request.ForumId);
			if (forum is null)
			{
				return Results.NotFoundById<Forum>(request.ForumId);
			}

			var result = forum.Delete(userAccessor.GetUserThrowIfIsNull(), request.comment);
			if (result.Success)
			{
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}

}
