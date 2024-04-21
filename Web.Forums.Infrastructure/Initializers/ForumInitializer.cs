namespace Web.Forums.Infrastructure.Initializers;

using Microsoft.Extensions.Options;
using Web.Forums.Domain.Aggregate;
using Web.Forums.Domain.Owned;
using Web.Forums.Infrastructure.EntityFrameworkCore;



public static class ForumInitializer
{
	public static void InitializeRoot(IServiceProvider serviceProvider)
	{
		using (var scope = serviceProvider.CreateScope())
		{
			IOptions<RootUserOptions> UserOptions = scope.ServiceProvider.GetRequiredService<IOptions<RootUserOptions>>();
			UserManager userManager = scope.ServiceProvider.GetRequiredService<UserManager>();

			var user = userManager.FindByNameAsync(UserOptions.Value.UserName).GetAwaiter().GetResult();

			var forumDbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
			forumDbContext.Database.EnsureCreated();


			if (!forumDbContext.Set<Forum>().Any())
			{
				var RootForum = Forum.RootForum(PrincipalUser.Root);
				forumDbContext.Add(RootForum);
				//forumDbContext.SaveChanges();

				InitializeForum(forumDbContext, RootForum, PrincipalUser.Root);
			}

		}
	}


	static string Title(int i) => $"Lorem ipsum [{i}].{Ulid.NewUlid().ToString()}";
	static string Text => 
		$"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do {Ulid.NewUlid().ToString()}";

	const int MainForums = 10;
	const int ThematicForums = 100;
	const int Announcements = 5;
	const int ThematicForumsTopic = 100;
	const int SubForums = 100;
	const int Topics = 10000;


	public static void InitializeForum(ForumDbContext forumDbContext, Forum RootForum, PrincipalUser User)
	{
		for (int mi = 0; mi < MainForums; mi++)
		{
			var mainForumResult = RootForum.CreateForum(User, Title(mi), Text);
			if(!mainForumResult.Success)
			{
				throw new Exception(mainForumResult.ToString());
			}
			var mainForum = mainForumResult.Value;

			//mainForum.Edit(User, Title(mi), Text);
			mainForum.AddCurator(User, Curator.Create(mainForum, User.UserId, User.UserName));
			mainForum.AddModerator(User, Moderator.Create(mainForum, User.UserId, User.UserName));
			forumDbContext.Forums.Add(mainForum);


			CreateThematicForums(forumDbContext, mainForumResult, User);
			forumDbContext.SaveChanges();
		}
	}


	private static void CreateThematicForums(ForumDbContext forumDbContext, Forum mainForum, PrincipalUser User)
	{
		for (int ti = 0; ti < ThematicForums; ti++)
		{
			var thematicForumResult = mainForum.CreateForum(User, Title(ti), Text);
			if (!thematicForumResult.Success)
			{
				throw new Exception(thematicForumResult.ToString());
			}
			var thematicForum = thematicForumResult.Value;
			//thematicForum.Edit(User, Title(ti), Text);
			thematicForum.AddCurator(User, Curator.Create(thematicForum, User.UserId, User.UserName));
			thematicForum.AddModerator(User, Moderator.Create(thematicForum, User.UserId, User.UserName));
			forumDbContext.Forums.Add(thematicForum);

			for (int tai = 0; tai < Announcements; tai++)
			{
				var ta = thematicForum.CreateAnnouncement(User, Title(tai), Text);
				if (!ta.Success)
				{
					throw new Exception(ta.ToString());
				}

				forumDbContext.Add(ta.Value);
			}


			for (int tti = 0; tti < ThematicForumsTopic; tti++)
			{
				var tft = thematicForum.CreateTopic(User, Title(tti), Text);
				if (!tft.Success)
				{
					throw new Exception(tft.ToString());
				}
				forumDbContext.Add(tft.Value);
			}


			for (int si = 0; si < SubForums; si++)
			{
				var subForum = thematicForum.CreateForum(User, Title(si), Text).Value;
				subForum.Edit(User, Title(si), Text);
				subForum.AddCurator(User, Curator.Create(mainForum, User.UserId, User.UserName));
				subForum.AddModerator(User, Moderator.Create(mainForum, User.UserId, User.UserName));
				forumDbContext.Add(subForum);

				var sa = subForum.CreateAnnouncement(User, Title(si), Text).Value;
				sa.Edit(User, Title(si), Text);
				forumDbContext.Add(sa);

				if (ti == 0 && si == 0)
				{
					for (int tti = 0; tti < Topics; tti++)
					{
						var ts = subForum.CreateTopic(User, Title(tti), Text).Value;
						forumDbContext.Add(ts);
					}
				}

			}

		}

	}
}
