﻿namespace Web.Forums.Infrastructure.Initializers;

using Mapster.Utils;
using Microsoft.Extensions.Options;
using Web.Forums.Domain.Aggregate;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Owned;
using Web.Forums.Infrastructure.EntityFrameworkCore;
using Z.EntityFramework.Extensions.EFCore;



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

			if (!forumDbContext.Set<Forum>().Any())
			{
				InitializeForum(forumDbContext);
				//forumDbContext.SaveChanges();
				forumDbContext.BulkSaveChanges();
			}

			//var mainForumCount = forumDbContext.Set<MainForum>().Count();
			//Console.WriteLine($"mainForumCount: {mainForumCount}");

			//var mainForum = forumDbContext.Set<MainForum>()
			//	.Include(x => x.ThematicForums)
			//	.First();
			//var ThematicForums = mainForum.ThematicForums.ToList();
			//Console.WriteLine($"ThematicForums: {ThematicForums.Count}");
		}
	}



	static string Title(int i) => $"Lorem ipsum dolor sit amet[{i}].{Ulid.NewUlid().ToString()}";
	static string Text => $"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. {Ulid.NewUlid().ToString()}";

	public static void InitializeForum(ForumDbContext forumDbContext)
	{
		PrincipalUser User = new(Guid.Empty, "Root", Enum.GetNames<ForumRoles>());

		var Root = Forum.RootForum();
		forumDbContext.Add(Root);


		for (int mi = 0; mi< 8;mi++)
		{
			var mainForumResult = Root.CreateForum(User, Title(mi), Text);
			if(!mainForumResult.Success)
			{
				throw new Exception(mainForumResult.ToString());
			}
			var mainForum = mainForumResult.Value;
			forumDbContext.Add(mainForum);

			Thread.Sleep(1);

			//ParentIsRoot
			/*
			for (int mai = 0; mai< 7; mai++)
			{
				var ma = mainForum.CreateAnnouncement(User, Title(mai), Text);
				if (!ma.Success)
				{
					throw new Exception(ma.message);
				}
				forumDbContext.Add(ma.Value);

				Thread.Sleep(1);
			}
			*/
			mainForum.Edit(User, Title(mi), Text);
			mainForum.AddCurator(User, new Curator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
			mainForum.AddModerator(User, new Moderator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));




			for (int ti = 0; ti < 10; ti++)
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

				Thread.Sleep(1);

				for (int tai = 0; tai< 5; tai++)
				{
					var ta = thematicForum.CreateAnnouncement(User, Title(tai), Text);
					if (!ta.Success)
					{
						throw new Exception(ta.ToString());
					}

					forumDbContext.Add(ta.Value);

					Thread.Sleep(1);
				}

				for (int tti = 0; tti < 50; tti++)
				{
					var tt = thematicForum.CreateTopic(User, Title(tti), Text);
					if (!tt.Success)
					{
						throw new Exception(tt.ToString());
					}
					forumDbContext.Add(tt.Value);

					Thread.Sleep(1);
				}


				for (int si = 0; si < 10; si++)
				{
					var subForum = thematicForum.CreateForum(User, Title(si), Text).Value;
					subForum.Edit(User, Title(mi), Text);
					subForum.AddCurator(User, new Curator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
					subForum.AddModerator(User, new Moderator(mainForum.ForumId, Ulid.NewUlid(), User.UserId, User.UserName));
					forumDbContext.Add(subForum);
					Thread.Sleep(1);

					var sa = subForum.CreateAnnouncement(User, Title(si), Text).Value;
					sa.Edit(User, Title(mi), Text);
					forumDbContext.Add(sa);
					Thread.Sleep(1);

					if (mi==0 && si == 0)
					{
						for (int tti = 0; tti < 1000; tti++)
						{
							var ts = subForum.CreateTopic(User, Title(tti), Text).Value;
							forumDbContext.Add(ts);
							Thread.Sleep(1);
						}
					}



				}
			}

		}
	}








}
