namespace Web.Areas.Identity.Pages.Account.UsersManager;


public class UsersManagerModel(UserManager userManager, ILogger<UsersManagerModel> logger): PageModel
{
    public IReadOnlyCollection<User> Users { get; set; } = new List<User>();

    public ActionResult OnGet() => Page();




    public record UserSearchByNameQuery
    {
        [Required]
        [BindProperty]
        [MinLength(4)]
        [MaxLength(25)]
        public string UserName { get; set; }
    }

    public UserSearchByNameQuery searchByNameQuery { get; set; }

    public async Task<ActionResult> OnPostByUserNameAsync(UserSearchByNameQuery searchByNameQuery)
    {
        //logger.LogInformation($"Post : {searchByNameQuery.UserName}");
        logger.LogInformation($"PostByUserName");

        if (ModelState.IsValid)
        {
            Users = await userManager.Users
                .AsNoTracking()
                .Where(x => x.UserName.Contains(searchByNameQuery.UserName))
                //.Include(x => x.UserRoles)
                .OrderBy(x => x.UserName)
                .ToListAsync();
        }

        return Page();
    }


    public record UserSearchByEmailQuery
    {
        [Required]
        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }
    }

    public UserSearchByEmailQuery searchByEmailQuery { get; set; }

    public async Task<ActionResult> OnPostByEmailAsync(UserSearchByEmailQuery searchByEmailQuery)
    {
        logger.LogInformation($"PostByEmail");

        if (ModelState.IsValid)
        {
            Users = await userManager.Users
                .AsNoTracking()
                .Where(x => x.Email.Contains(searchByEmailQuery.Email))
                //.Include(x => x.UserRoles)
                .OrderBy(x => x.Email)
                .ToListAsync();
        }

        return Page();
    }







}
