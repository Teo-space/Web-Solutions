namespace Web.Controllers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Web.Forums.Domain.Aggregate;
using Web.Forums.Infrastructure.EntityFrameworkCore;
using Web.Forums.UseCases.Forums.ReadModel;
using Web.Forums.UseCases.Forums.WriteModel;
using Web.Models;


public class ForumsController
(
	ILogger<ForumsController> logger,
	ForumDbContext forumDbContext,
	IMediator mediatr
)
	: Controller
{

	//01H91G1S5HPYYG8Z218MA0GC0N
	//00000000000000000000000000

	public async Task<IActionResult> Index(QueryForumDisplay request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		return await ForumDisplay(request);
	}

	public async Task<IActionResult> ForumDisplay(QueryForumDisplay request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		var result = await mediatr.Send(request);
		return View("ForumDisplay", result);
	}
	public async Task<IActionResult> ForumDisplayNextPage(QueryForumDisplayNextPage request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		var result = await mediatr.Send(request);
		return View("ForumDisplay", result);
	}

	public async Task<IActionResult> ForumDisplayPreviousPage(QueryForumDisplayPreviousPage request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		var result = await mediatr.Send(request);
		return View("ForumDisplay", result);
	}

	public async Task<IActionResult> ForumDisplayPage(QueryForumDisplayPage request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		var result = await mediatr.Send(request);
		return View("ForumDisplay", result);
	}

	public async Task<IActionResult> ForumSearch(QueryForumSearch request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		throw new NotImplementedException();
	}






	public async Task<IActionResult> ForumCreate(CommandForumCreate request)
	{
		logger?.LogWarning($"ForumCreate[{this.HttpContext.Request.Path}][{this.HttpContext.Request.Method}] {request.ParentForumId}, {request.Title}, {request.Description}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		ViewBag.ForumResult = await mediatr.Send(new QueryForumGet(request.ParentForumId));
		return View(nameof(ForumCreate), request);
	}


	[HttpPost]
	public async Task<IActionResult> ForumCreatePost(CommandForumCreate request)
	{
		logger?.LogWarning($"ForumCreatePost[{this.HttpContext.Request.Path}][{this.HttpContext.Request.Method}] {request.ParentForumId}, {request.Title}, {request.Description}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		if(!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumCreate), request);
		}

		var ForumResult = await mediatr.Send(request);
		if (ForumResult.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(ForumResult.Value.ForumId));
		}
		else
		{
			return RedirectToAction(nameof(ForumCreate), request);
		}
	}








	public async Task<IActionResult> ForumEdit(CommandForumEdit request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}///{this.HttpContext.Request.Method} {request.ForumId}, {request.Title}, {request.Description}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		ViewBag.ForumResult = await mediatr.Send(new QueryForumGet(request.ForumId));
		return View(request);
	}

	[HttpPost]
	public async Task<IActionResult> ForumEditPost(CommandForumEdit request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}///{this.HttpContext.Request.Method} {request.ForumId}, {request.Title}, {request.Description}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumEdit), request);
		}

		var ForumResult = await mediatr.Send(request);
		if (ForumResult.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(ForumResult.Value.ForumId));
		}
		else
		{
			return RedirectToAction(nameof(ForumEdit), request);
		}
	}









	public async Task<IActionResult> ForumOpen(CommandForumOpen request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	{Result}");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
	}

	public async Task<IActionResult> ForumClose(CommandForumClose request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
	}

	public async Task<IActionResult> ForumDelete(CommandForumDelete request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
	}

	public async Task<IActionResult> ForumUnDelete(CommandForumUnDelete request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		var Result = await mediatr.Send(request);
		
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
	}










	public async Task<IActionResult> ForumAddCurator(CommandForumAddCurator request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			ViewBag.ForumResult = Result.InputValidationError<Forum>(request.ForumId.ToString());
			return View(nameof(ForumCreate), request);
		}
		var forum = await forumDbContext.Forums.Where(f => f.ForumId == request.ForumId).FirstOrDefaultAsync();
		if (forum == null)
		{
			return View(Result.NotFound<Forum>(request.ForumId.ToString()));
		}
		var result = forum.AddCurator(User, new Forums.Domain.Owned.Curator(
			forum.ForumId, Ulid.NewUlid(), request.UserId, request.UserName));

		if (result.Success)
		{
			await forumDbContext.SaveChangesAsync();
		}
		return View(result);
	}

	public async Task<IActionResult> ForumRemoveCurator(CommandForumRemoveCurator request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			ViewBag.ForumResult = Result.InputValidationError<Forum>(request.ForumId.ToString());
			return View(nameof(ForumCreate), request);
		}
		var forum = await forumDbContext.Forums.Where(f => f.ForumId == request.ForumId).FirstOrDefaultAsync();
		if (forum == null)
		{
			return View(Result.NotFound<Forum>(request.ForumId.ToString()));
		}
		var result = forum.RemoveCurator(User, request.UserId);
		if (result.Success)
		{
			await forumDbContext.SaveChangesAsync();
		}
		return View(result);
	}

	public async Task<IActionResult> ForumAddModerator(CommandForumAddModerator request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			ViewBag.ForumResult = Result.InputValidationError<Forum>(request.ForumId.ToString());
			return View(nameof(ForumCreate), request);
		}
		var forum = await forumDbContext.Forums.Where(f => f.ForumId == request.ForumId).FirstOrDefaultAsync();
		if (forum == null)
		{
			return View(Result.NotFound<Forum>(request.ForumId.ToString()));
		}
		var result = forum.AddModerator(User, new Forums.Domain.Owned.Moderator(
			forum.ForumId, Ulid.NewUlid(), request.UserId, request.UserName));
		if (result.Success)
		{
			await forumDbContext.SaveChangesAsync();
		}
		return View(result);
	}

	public async Task<IActionResult> ForumRemoveModerator(CommandForumRemoveModerator request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			ViewBag.ForumResult = Result.InputValidationError<Forum>(request.ForumId.ToString());
			return View(nameof(ForumCreate), request);
		}
		var forum = await forumDbContext.Forums.Where(f => f.ForumId == request.ForumId).FirstOrDefaultAsync();
		if (forum == null)
		{
			return View(Result.NotFound<Forum>(request.ForumId.ToString()));
		}
		var result = forum.RemoveModerator(User, request.UserId);
		if (result.Success)
		{
			await forumDbContext.SaveChangesAsync();
		}
		return View(result);
	}















	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}