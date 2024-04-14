namespace Web.Areas.Identity.Pages.Account.Manage;


public class ResetAuthenticatorModel(
    UserManager userManager,
    SignInManager signInManager,
    ILogger<ResetAuthenticatorModel> logger)
    : PageModel
{

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            logger.LogInformation($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            logger.LogInformation($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        await userManager.SetTwoFactorEnabledAsync(user, false);
        await userManager.ResetAuthenticatorKeyAsync(user);
        var userId = await userManager.GetUserIdAsync(user);
        logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

        await signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

        return RedirectToPage("./EnableAuthenticator");
    }
}
