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
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicDisplay), request);
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"Success");
			return RedirectToAction(nameof(TopicDisplay), request);
		}
		else
		{
			logger?.LogError($"Failed");
			return RedirectToAction(nameof(TopicDisplay), request);
		}
	}




	public async Task<IActionResult> TopicCreate(CommandTopicCreate request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		ViewBag.ParentForumResult = await mediatr.Send(new QueryForumGet(request.ForumId));
		return View(nameof(TopicCreate), request);
	}


	[HttpPost]
	public async Task<IActionResult> TopicCreatePost(CommandTopicCreate request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicCreate), request);
		}

		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(Result.Value.TopicId));
		}
		else
		{
			return RedirectToAction(nameof(TopicCreate), request);
		}
	}



	public async Task<IActionResult> TopicEdit(CommandTopicEdit request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogWarning($"ModelState.IsValid [{ModelState.IsValid}]");

		ViewBag.TopicResult = await mediatr.Send(new QueryTopicDisplay(request.TopicId));
		return View(nameof(TopicEdit), request);
	}

	[HttpPost]
	public async Task<IActionResult> TopicEditPost(CommandTopicEdit request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		logger?.LogError($"ModelState.IsValid [{ModelState.IsValid}]");

		if (!ModelState.IsValid)
		{
			return RedirectToAction(nameof(TopicEdit), request);
		}
		var TopicResult = await mediatr.Send(request);
		if (TopicResult.Success)
		{
			logger?.LogWarning($"Success");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(TopicResult.Value.TopicId));
		}
		else
		{
			logger?.LogError(TopicResult);
			return RedirectToAction(nameof(TopicEdit), request);
		}
	}



	public async Task<IActionResult> TopicClose(CommandTopicClose request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	!ModelState.IsValid");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
	}

	public async Task<IActionResult> TopicOpen(CommandTopicOpen request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	!ModelState.IsValid");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
	}




	public async Task<IActionResult> TopicDelete(CommandTopicDelete request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	!ModelState.IsValid");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
	}

	public async Task<IActionResult> TopicUnDelete(CommandTopicUnDelete request)
	{
		logger?.LogWarning($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}");
		if (!ModelState.IsValid)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	!ModelState.IsValid");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		var Result = await mediatr.Send(request);
		if (Result.Success)
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result.Success");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
		else
		{
			logger?.LogError($"{this.HttpContext.Request.Path}	{this.HttpContext.Request.Method}	Result: {Result}");
			return RedirectToAction(nameof(TopicDisplay), new QueryTopicDisplay(request.TopicId));
		}
	}




}
