namespace Web.Forums.UseCases.Forums.ReadModel;


public record QueryForumSearch(string SearchString)

	///На самом деле будет возвращать тип- результаты поиска
	: IRequest<Result<IReadOnlyCollection<Forum>>>
{
	public class Validator : AbstractValidator<QueryForumSearch>
	{
		public Validator()
		{
			RuleFor(x => x.SearchString).NotNull().NotEmpty().MinimumLength(5).MinimumLength(40);
		}
	}
}
