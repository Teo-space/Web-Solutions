using Web.Forums.UseCases.Posts.WriteModel;

namespace Web.Controllers;

public class PostsController(ILogger<PostsController> logger, IMediator mediatr) : Controller
{
	public IActionResult Index()
	{
		return View();
	}



	[HttpPost]
	public async Task<IActionResult> PostCreatePost(CommandPostCreate request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicsController.TopicDisplay), "Topics", request);
		}

		var result = await mediatr.Send(request);
		if (result.Success)
		{
			logger?.LogInformation("Created");
			return RedirectToAction(nameof(TopicsController.TopicDisplay), "Topics", request);
		}
		logger?.LogError("Result: {Result}", result);
		return View("PostError", request);
	}


}
