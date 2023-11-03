namespace Web.Controllers;


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
	//Ulid Id
	//01H91G1S5HPYYG8Z218MA0GC0N
	//00000000000000000000000000

	public Task<IActionResult> Index(QueryForumDisplay request) 
		=> ForumDisplay(request);

	public async Task<IActionResult> ForumDisplay(QueryForumDisplay request) 
		=> View("ForumDisplay", await mediatr.Send(request));

	public async Task<IActionResult> ForumDisplayNextPage(QueryForumDisplayNextPage request) 
		=> View("ForumDisplay", await mediatr.Send(request));


	public async Task<IActionResult> ForumDisplayPreviousPage(QueryForumDisplayPreviousPage request)
		=> View("ForumDisplay", await mediatr.Send(request));


	public async Task<IActionResult> ForumDisplayPage(QueryForumDisplayPage request)
		=> View("ForumDisplay", await mediatr.Send(request));

	public async Task<IActionResult> ForumSearch(QueryForumSearch request) 
		=> throw new NotImplementedException();







	public async Task<IActionResult> ForumCreate(CommandForumCreate request)
	{
		ViewBag.ForumResult = await mediatr.Send(new QueryForumGet(request.ParentForumId));
		return View(nameof(ForumCreate), request);
	}


	[HttpPost]
	public async Task<IActionResult> ForumCreatePost(CommandForumCreate request)
	{
		if(!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumCreate), request);
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(Result.Value.ForumId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(ForumCreate), request);
	}








	public async Task<IActionResult> ForumEdit(CommandForumEdit request)
	{
		ViewBag.ForumResult = await mediatr.Send(new QueryForumGet(request.ForumId));
		return View(request);
	}


	[HttpPost]
	public async Task<IActionResult> ForumEditPost(CommandForumEdit request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumEdit), request);
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(Result.Value.ForumId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(ForumEdit), request);
	}









	public async Task<IActionResult> ForumOpen(CommandForumOpen request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
	}

	public async Task<IActionResult> ForumClose(CommandForumClose request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
	}


	public async Task<IActionResult> ForumDelete(CommandForumDelete request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
	}


	public async Task<IActionResult> ForumUnDelete(CommandForumUnDelete request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(ForumDisplay), new QueryForumDisplay(request.ForumId));
	}










	public async Task<IActionResult> ForumAddCurator(CommandForumAddCurator request)
	{
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
		var result = forum.AddCurator(User, Curator.Create(forum.ForumId, request.UserId, request.UserName));
		if (result.Success)
		{
			await forumDbContext.SaveChangesAsync();
		}
		logger.LogError("Result: {result}", result);
		return View(result);
	}

	public async Task<IActionResult> ForumRemoveCurator(CommandForumRemoveCurator request)
	{
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
		logger.LogError("Result: {result}", result);
		return View(result);
	}

	public async Task<IActionResult> ForumAddModerator(CommandForumAddModerator request)
	{
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
		logger.LogError("Result: {result}", result);
		return View(result);
	}

	public async Task<IActionResult> ForumRemoveModerator(CommandForumRemoveModerator request)
	{
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
		logger.LogError("Result: {result}", result);
		return View(result);
	}















	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}