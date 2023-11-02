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
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		var result = await mediatr.Send(request);
		return View(nameof(AnnouncementDisplay), result);
	}


	/// <summary>
	/// CommandAnnouncementCreate(IDType ForumId, string Title, string Text)
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	public async Task<IActionResult> AnnouncementCreate(CommandAnnouncementCreate request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		ViewBag.ParentForumResult = await mediatr.Send(new QueryForumGet(request.ForumId));
		return View(nameof(AnnouncementCreate), request);
	}

	/// <summary>
	/// CommandAnnouncementCreate(IDType ForumId, string Title, string Text)
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> AnnouncementCreatePost(CommandAnnouncementCreate request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(AnnouncementCreate), request);
		}

		var AnnouncementResult = await mediatr.Send(request);
		if (AnnouncementResult.Success)
		{
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(AnnouncementResult.Value.AnnouncementId));
		}
		else
		{
			return RedirectToAction(nameof(AnnouncementCreate), request);
		}
	}



	public async Task<IActionResult> AnnouncementEdit(CommandAnnouncementEdit request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		ViewBag.AnnouncementResult = await mediatr.Send(new QueryAnnouncementGet(request.AnnouncementId));
		return View(nameof(AnnouncementEdit), request);
	}

	[HttpPost]
	public async Task<IActionResult> AnnouncementEditPost(CommandAnnouncementEdit request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogError($"ModelState.IsValid [{ModelState.IsValid}]");

		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(AnnouncementEdit), request);
		}
		var AnnouncementResult = await mediatr.Send(request);
		if (AnnouncementResult.Success)
		{
			logger?.LogWarning($"Success");
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(AnnouncementResult.Value.AnnouncementId));
		}
		else
		{
			logger?.LogError($"{AnnouncementResult}");
			return RedirectToAction(nameof(AnnouncementEdit), request);
		}
	}



	public async Task<IActionResult> AnnouncementDelete(CommandAnnouncementDelete request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	!ModelState.IsValid");
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
	}

	public async Task<IActionResult> AnnouncementUnDelete(CommandAnnouncementUnDelete request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	!ModelState.IsValid");
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(AnnouncementDisplay), new QueryAnnouncementDisplay(request.AnnouncementId));
		}
	}







}
