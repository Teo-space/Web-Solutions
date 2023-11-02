namespace Web.Wiki.Domain;

/// <summary>
/// Статья
/// </summary>
public class Article
{
	public Ulid ArticleId { get; private set; }
	public Article Main { get; private set; }
	public Ulid ArticleVersionId { get; private set; }

	public string Title { get; private set; }
	public string Description { get; private set; }
	public string Text { get; private set; }

	public long Views { get; private set; }
	public void Viewed() => Views++;



	public UserAction CreatedBy { get; private set; }


	public bool Approved { get; private set; } = false;
	public UserAction ApprovedBy { get; private set; }
	public bool BaseVersion { get; private set; } = false;
	public UserAction BaseVersionBy { get; private set; }
	public bool Deleted { get; private set; } = false;
	public UserAction DeletedBy { get; private set; }


	public List<Article> ArticleVersions { get; private set; } = new List<Article>();

	/// <summary>
	/// Создается главная версия (первая), Я бы назвал ее базовой, но базовая это текущая
	/// </summary>
	/// <param name="Title"></param>
	/// <param name="Description"></param>
	/// <param name="Text"></param>
	/// <returns></returns>
	public static Article Create(string Title, string Description, string Text)
	{
		var article = new Article();
		article.ArticleId = new Ulid();
		//Первая версия имеет одинаковые ArticleId и ArticleVersionId
		article.ArticleVersionId = article.ArticleId;

		article.Title = Title;
		article.Description = Description;
		article.Text = Text;
		//article.CreatedBy
		return article;
	}

	public Article CreateVersion(string Title, string Description, string Text)
	{
		var article = new Article();
		article.Main = this;
		article.ArticleId = this.ArticleId;
		article.ArticleVersionId = new Ulid();

		article.Title = Title;
		article.Description = Description;
		article.Text = Text;
		//article.CreatedBy
		return article;
	}

	public Article Approve()
	{
		this.Approved = true;
		//ApprovedBy
		return this;
	}
	public Article SetAsBaseVersion()
	{
		this.BaseVersion = true;
		//BaseVersionBy
		return this;
	}

	public Article Delete()
	{
		this.Deleted = true;
		//DeletedBy
		return this;
	}







}