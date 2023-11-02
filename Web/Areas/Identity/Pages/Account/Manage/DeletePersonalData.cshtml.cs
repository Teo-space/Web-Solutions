// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


namespace Web.Areas.Identity.Pages.Account.Manage;


public class DeletePersonalDataModel(
        UserManager userManager,
        SignInManager signInManager,
        ILogger<DeletePersonalDataModel> logger)
    : PageModel
{
    public class InputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; }



    public bool RequirePassword { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            logger.LogInformation($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        RequirePassword = await userManager.HasPasswordAsync(user);
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

        RequirePassword = await userManager.HasPasswordAsync(user);
        if (RequirePassword)
        {
            if (!await userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return Page();
            }
        }

        var result = await userManager.DeleteAsync(user);
        var userId = await userManager.GetUserIdAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Unexpected error occurred deleting user.");
        }

        await signInManager.SignOutAsync();

        logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

        return Redirect("~/");
    }
}
