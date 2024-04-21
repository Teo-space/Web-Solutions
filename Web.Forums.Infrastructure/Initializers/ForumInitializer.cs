namespace Web.Forums.Infrastructure.Initializers;

using Microsoft.Extensions.Options;
using Web.Forums.Domain.Aggregate;
using Web.Forums.Domain.Enums;
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
				InitializeForum(forumDbContext);
				forumDbContext.SaveChanges();
			}

		}
	}


	static string Title(int i) => $"Lorem ipsum [{i}].{Ulid.NewUlid().ToString()}";
	static string Text => 
		$"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do {Ulid.NewUlid().ToString()}";

	public static void InitializeForum(ForumDbContext forumDbContext)
	{
		PrincipalUser User = new(Guid.Empty, "Root", Enum.GetNames<ForumRoles>());

		var Root = Forum.RootForum();
		forumDbContext.Add(Root);


		for (int mi = 0; mi < 10; mi++)
		{
			var mainForumResult = Root.CreateForum(User, Title(mi), Text);
			if(!mainForumResult.Success)
			{
				throw new Exception(mainForumResult.ToString());
			}
			var mainForum = mainForumResult.Value;
			forumDbContext.Add(mainForum);

			mainForum.Edit(User, Title(mi), Text);
			mainForum.AddCurator(User, new Curator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
			mainForum.AddModerator(User, new Moderator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));

			for (int ti = 0; ti < 100; ti++)
			{
				var thematicForumResult = mainForum.CreateForum(User, Title(ti), Text);
				if (!thematicForumResult.Success)
				{
					throw new Exception(thematicForumResult.ToString());
				}
				var thematicForum = thematicForumResult.Value;
				thematicForum.Edit(User, Title(mi), Text);
				thematicForum.AddCurator(User, new Curator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
				thematicForum.AddModerator(User, new Moderator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
				forumDbContext.Add(thematicForum);

				for (int tai = 0; tai< 10; tai++)
				{
					var ta = thematicForum.CreateAnnouncement(User, Title(tai), Text);
					if (!ta.Success)
					{
						throw new Exception(ta.ToString());
					}

					forumDbContext.Add(ta.Value);
				}

				for (int tti = 0; tti < 100; tti++)
				{
					var tt = thematicForum.CreateTopic(User, Title(tti), Text);
					if (!tt.Success)
					{
						throw new Exception(tt.ToString());
					}
					forumDbContext.Add(tt.Value);
				}


				for (int si = 0; si < 100; si++)
				{
					var subForum = thematicForum.CreateForum(User, Title(si), Text).Value;
					subForum.Edit(User, Title(mi), Text);
					subForum.AddCurator(User, new Curator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
					subForum.AddModerator(User, new Moderator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
					forumDbContext.Add(subForum);

					var sa = subForum.CreateAnnouncement(User, Title(si), Text).Value;
					sa.Edit(User, Title(mi), Text);
					forumDbContext.Add(sa);

					if (mi==0 && si == 0)
					{
						for (int tti = 0; tti < 10000; tti++)
						{
							var ts = subForum.CreateTopic(User, Title(tti), Text).Value;
							forumDbContext.Add(ts);
						}
					}

				}
			}

		}
	}








}
