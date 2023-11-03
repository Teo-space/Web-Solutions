namespace Web.Controllers;


public class TopicsController(ILogger <TopicsController> logger, IMediator mediatr) : Controller
{
	public async Task<IActionResult> Index(QueryTopicDisplay request) => await TopicDisplay(request);

	public async Task<IActionResult> TopicDisplay(QueryTopicDisplay request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");

		var result = await mediatr.Send(request);
		ViewBag.TopicResult = result;
		return View(nameof(TopicDisplay), new CommandPostCreate(request.TopicId, string.Empty));
	}



	[HttpPost]
	public async Task<IActionResult> TopicPostCreatePost(CommandPostCreate request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicDisplay), request);
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), request);
		}
		logger?.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(TopicDisplay), request);
	}




	public async Task<IActionResult> TopicCreate(CommandTopicCreate request)
	{
		ViewBag.ParentForumResult = await mediatr.Send(new QueryForumGet(request.ForumId));
		return View(nameof(TopicCreate), request);
	}


	[HttpPost]
	public async Task<IActionResult> TopicCreatePost(CommandTopicCreate request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicCreate), request);
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(Result.Value.TopicId));
		}
		logger?.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(TopicCreate), request);
	}



	public async Task<IActionResult> TopicEdit(CommandTopicEdit request)
	{
		ViewBag.TopicResult = await mediatr.Send(new QueryTopicDisplay(request.TopicId));
		return View(nameof(TopicEdit), request);
	}

	[HttpPost]
	public async Task<IActionResult> TopicEditPost(CommandTopicEdit request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicEdit), request);
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(Result.Value.TopicId));
		}
		logger?.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(TopicEdit), request);
	}



	public async Task<IActionResult> TopicClose(CommandTopicClose request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		logger?.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
	}

	public async Task<IActionResult> TopicOpen(CommandTopicOpen request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		logger?.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
	}




	public async Task<IActionResult> TopicDelete(CommandTopicDelete request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		logger?.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
	}

	public async Task<IActionResult> TopicUnDelete(CommandTopicUnDelete request)
	{
		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		logger?.LogError("Result: {Result}", Result);
		return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
	}




}
