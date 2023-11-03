namespace Web.Controllers;

using Web.Forums.Domain.Aggregate;
using Web.Forums.UseCases.Announcements.ReadModel;
using Web.Forums.UseCases.Announcements.WriteModel;
using Web.Forums.UseCases.Forums.ReadModel;


public class AnnouncementsController(ILogger <AnnouncementsController> logger, IMediator mediatr) : Controller
{

	public async Task<IActionResult> Index(QueryAnnouncementDisplay request) => await AnnouncementDisplay(request);
	public async Task<IActionResult> AnnouncementDisplay(QueryAnnouncementDisplay request)
	{
		var result = await mediatr.Send(request);
		return View(nameof(AnnouncementDisplay), result);
	}


	public async Task<IActionResult> AnnouncementCreate(CommandAnnouncementCreate request)
	{
		ViewBag.ParentForumResult = await mediatr.Send(new QueryForumGet(request.ForumId));
		return View(nameof(AnnouncementCreate), request);
	}

	[HttpPost]
	public async Task<IActionResult> AnnouncementCreatePost(CommandAnnouncementCreate request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(AnnouncementCreate), request);
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(Result.Value.AnnouncementId));
		}
		return RedirectToAction(nameof(AnnouncementCreate), request);
	}



	public async Task<IActionResult> AnnouncementEdit(CommandAnnouncementEdit request)
	{
		ViewBag.AnnouncementResult = await mediatr.Send(new QueryAnnouncementGet(request.AnnouncementId));
		return View(nameof(AnnouncementEdit), request);
	}

	[HttpPost]
	public async Task<IActionResult> AnnouncementEditPost(CommandAnnouncementEdit request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(AnnouncementEdit), request);
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(Result.Value.AnnouncementId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(AnnouncementEdit), request);
	}



	public async Task<IActionResult> AnnouncementDelete(CommandAnnouncementDelete request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
	}

	public async Task<IActionResult> AnnouncementUnDelete(CommandAnnouncementUnDelete request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		logger.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
	}







}
