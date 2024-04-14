using Web.Forums.UseCases.Forums.Curators;
using Web.Forums.UseCases.Forums.Curators.ReadModel;
using Web.Forums.UseCases.Posts.WriteModel;

namespace Web.Controllers;

[Authorize]
public class ForumCuratorsController(ILogger<ForumCuratorsController> logger, IMediator mediatr) : Controller
{

	public async Task<IActionResult> Index(QueryForumCuratorsDisplay request) => await ForumCuratorsDisplay(request);

	public async Task<IActionResult> ForumCuratorsDisplay(QueryForumCuratorsDisplay request)
	{
		var result = await mediatr.Send(request);
		ViewBag.ForumResult = result;
		return View(nameof(ForumCuratorsDisplay), new CommandForumCuratorAdd(request.ForumId, Guid.NewGuid(), string.Empty));
	}





}
