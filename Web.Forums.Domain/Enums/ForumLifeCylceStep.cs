namespace Web.Forums.Domain.Enums;

/// <summary>
/// Шаги жизненного цикла форума
/// </summary>
public enum ForumLifeCylceStep
{
	None = 0,
	Created = 1000,
	Closed = int.MaxValue,
	Deleted = int.MinValue,
}

