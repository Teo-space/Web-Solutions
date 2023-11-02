namespace Web.Areas.Identity.Pages.Account.UsersManager;

public class UserProfileModel(UserManager userManager, ILogger<UserProfileModel> logger) : PageModel
{
    public record Query(Guid UserId, string UserName, string Email)
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotNull().NotEmpty();
                RuleFor(x => x.UserName).NotNull().NotEmpty().MinimumLength(4).MaximumLength(25);
                RuleFor(x => x.UserName).NotNull().NotEmpty().EmailAddress();
            }
        }
    }

    [BindProperty(SupportsGet = true)]
    public Query UserModel { get; set; }


    public List<Role> UserRoles { get; private set; } = new List<Role>();

    public async Task<ActionResult> OnGet()
    {
        var user = await userManager.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == UserModel.Email);
        if (user == null)
        {
            logger.LogWarning("User not found!");
            return Page();
        }

        UserRoles = user.UserRoles
            .Select(x => x.Role)
            .OrderBy(x => x.Name)
            .ToList();

        return Page();
    }


}
