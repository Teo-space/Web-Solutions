using System.Security.Claims;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Forum
{
	public static int TopicsPageSize { get; } = 30;
	public bool TopicsReadonly { get; private set; } = false;
	//public uint TopicsCount { get; set; } = 0;
	public List<Topic> Topics { get; private set; } = new List<Topic>();

	public ulong TopicsCount { get; private set; } = 0;
	public ulong TopicCreated() => TopicsCount++;


	/// <summary>
	/// Проверка на право создания топика
	/// </summary>
	public bool CanCreateTopic(PrincipalUser user)
	{
		return !(IsRoot || ParentIsRoot) && (IsAdmin(user) || (!Closed && IsCurator(user)) || !TopicsReadonly);
	}

	public Result<Topic> CreateTopic(ClaimsPrincipal principal, string Title, string Text)
		=> CreateTopic(new PrincipalUser(principal), Title, Text);
	public Result<Topic> CreateTopic(PrincipalUser user, string Title, string Text)
	{
		if (CanCreateTopic(user))
		{
			var topic = Topic.Create(this, user, Title, Text);

			Topics.Add(topic);
			this.TopicCreated();

			return Results.Ok(topic);
		}
		else
		{
			return Results.NotEnoughPermissions<Topic>();
		}
	}


}
