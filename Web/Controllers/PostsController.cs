using Microsoft.Extensions.Logging;

namespace Web.Controllers;

public class PostsController(ILogger<PostsController> logger, IMediator mediatr) : Controller
{
	public IActionResult Index()
	{
		return View();
	}


	//public async Task<IActionResult> TopicCreate(CommandPostCreate request)
	//{
	//	logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
	//	logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

	//	ViewBag.ParentForumResult = await mediatr.Send(new QueryForumGet(request.TopicId));
	//	return View(nameof(TopicCreate), nameof(TopicsController), request);
	//}


	//[HttpPost]
	//public async Task<IActionResult> TopicCreatePost(CommandPostCreate request)
	//{
	//	logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
	//	logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

	//	if (!ModelState.IsValid)
	//	{
	//		return RedirectToAction(nameof(TopicCreate), request);
	//	}

	//	var Result = await mediatr.Send(request);
	//	if (Result.Success)
	//	{
	//		return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(Result.Value.TopicId));
	//	}
	//	else
	//	{
	//		return RedirectToAction(nameof(TopicCreate), request);
	//	}
	//}




























}
