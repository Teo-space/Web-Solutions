namespace Web.Forums.Domain.Owned;

public record Hierarhy
{
	/// <summary>
	/// Current element materialized path
	/// </summary>
	public string Path { get; protected init; }
	/// <summary>
	/// Parent element materialized path
	/// </summary>
	public string ParentPath { get; protected init; }
	/// <summary>
	/// element name for materialized path
	/// </summary>
	public string PathName { get; protected init; }

	/// <summary>
	/// Разделитель в materialized path
	/// </summary>
	public const char PathSeparator = '.';

	/// <summary>
	/// Create Hierarhy Root
	/// </summary>
	public static Hierarhy CreateRoot(string pathName)
	{
		var hierarhy = new Hierarhy()
		{
			PathName = pathName,
			ParentPath = default,
			Path = pathName,
		};

		return hierarhy;
	}

	/// <summary>
	/// Create child element
	/// </summary>
	public static Hierarhy CreateChild(Hierarhy parent, string pathName)
	{
		var hierarhy = new Hierarhy()
		{
			PathName = pathName,
			ParentPath = parent.Path,
			Path = $"{parent.Path}{PathSeparator}{pathName}",
		};

		return hierarhy;
	}

	/// <summary>
	/// Смена родительского узла. 
	/// Потребуется пройтись по всем дочерним узлам для обновления.
	/// 0) Выбрать нового родителя 'parent'
	/// 1) Выбрать элемент который перемещаем 'target'
	/// 2) Выбрать дочерние элементы "SELECT WHERE ParentPath = target.Path" "Childs"
	/// 3) Обновить элемент который перемещаем target.UpdateParent(parent)
	/// 3.1) target.ParentId = parent.Id
	/// 4) Обновить дочерние элементы Childs.UpdateParent(target)
	/// 5) SaveChanges();
	/// </summary>
	public Hierarhy UpdateParent(Hierarhy parent)
	{
		return this with
		{
			ParentPath = parent.Path,
			Path = $"{parent.Path}{PathSeparator}{PathName}"
		};
	}
}
