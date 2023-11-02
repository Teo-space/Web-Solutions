namespace Web.Areas.Identity.Pages.Account.UsersManager;


public class UserRolesManager(
    UserManager userManager,
    RoleManager roleManager,
    ILogger<UserRolesManager> logger)

    : PageModel
{
    [BindProperty]
    public string UserId { get; set; }
    [BindProperty]
    public string UserName { get; set; }
    [BindProperty]
    public string Email { get; set; }




    public record UserInRole(Guid Id, string Name, bool InRole);

    [BindProperty]
    public List<UserInRole> UserRoles { get; private set; } = new List<UserInRole>();

    public async Task<ActionResult> OnGetAsync(string UserId, string UserName, string Email)
    {
        this.UserId = UserId;
        this.UserName = UserName;
        this.Email = Email;

        var user = await userManager.FindByEmailAsync(Email);
        if (user == null)
        {
            logger.LogWarning("User not found!");
            return Page();
        }

        await UpdateRolesForUser(user);
        return Page();
    }

    async Task UpdateRolesForUser(User user)
    {
        var Roles = await roleManager.Roles.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        List<UserInRole> UserRoles = new();
        foreach (var role in Roles)
        {
            UserRoles.Add(new(role.Id, role.Name, await userManager.IsInRoleAsync(user, role.Name)));
        }
        this.UserRoles = UserRoles;
    }


    public bool RolesChanged = false;

    public async Task<ActionResult> OnPostAsync()
    {
        logger.LogInformation($"OnPost  {UserRoles.Count()}");

        var user = await userManager.FindByEmailAsync(Email);
        if (user == null)
        {
            logger.LogWarning("User not found!");
            return Page();
        }

        foreach (var role in UserRoles)
        {
            logger.LogWarning(role.Name);

            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                logger.LogWarning("Role not Exists");
                return Page();
            }
        }

        foreach (var roleToAdd in UserRoles.Where(x => x.InRole).Select(x => x.Name))
        {
            if (!await userManager.IsInRoleAsync(user, roleToAdd))
            {
                await userManager.AddToRoleAsync(user, roleToAdd);
            }
        }
        foreach (var roleRemove in UserRoles.Where(x => !x.InRole).Select(x => x.Name))
        {
            if (await userManager.IsInRoleAsync(user, roleRemove))
            {
                await userManager.RemoveFromRoleAsync(user, roleRemove);
            }
        }


        RolesChanged = true;

        await UpdateRolesForUser(user);
        return Page();
    }





}
