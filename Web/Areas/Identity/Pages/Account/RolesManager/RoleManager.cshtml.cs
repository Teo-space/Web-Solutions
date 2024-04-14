namespace Web.Areas.Identity.Pages.Account.RolesManager;



public class RoleManagerModel(RoleManager roleManager, ILogger<RoleManagerModel> logger): PageModel
{
    public IReadOnlyCollection<Role> Roles { get; set; } = new List<Role>();

	public async Task OnGetAsync() => await UpdateRoles();
	async Task UpdateRoles() => Roles = await roleManager.Roles.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    

    [Required]
    [BindProperty]
    [MinLength(4)]
    [MaxLength(25)]
    public string RoleName { get; set; } = string.Empty;

    public async Task OnPostCreateRoleAsync()
    {
        logger.LogInformation($"OnPostCreateRoleAsync");

        if (string.IsNullOrEmpty(RoleName))
        {
            logger.LogInformation($"RoleName Empty");
            ModelState.AddModelError(string.Empty, "RoleName Empty");
        }

        if (await roleManager.RoleExistsAsync(RoleName))
        {
            logger.LogInformation($"Role already Exists!!");
            ModelState.AddModelError(string.Empty, "Role already Exists!!");
        }

        var Result = await roleManager.CreateAsync(new Role(RoleName));
        if (Result.Succeeded)
        {
            logger.LogInformation($"Role Created");
        }
        else
        {
            foreach (var err in Result.Errors)
            {
                logger.LogInformation(err.ToString());
            }
        }
        RoleName = string.Empty;
        await UpdateRoles();
    }



	[Required]
	[BindProperty]
	public string DeleteRoleId { get; set; } = string.Empty;

    public async Task OnPostDeleteAsync()
    {
        logger.LogInformation($"OnPostDeleteAsync");

        if (!string.IsNullOrEmpty(DeleteRoleId))
        {
            var Role = await roleManager.FindByIdAsync(DeleteRoleId);
            if (Role != null)
            {
                await roleManager.DeleteAsync(Role);
                logger.LogInformation($"Role deleted {Role.Name}");
            }
            else
            {
                logger.LogInformation($"Role not found");
            }
            DeleteRoleId = string.Empty;
        }

        await UpdateRoles();
    }

}
