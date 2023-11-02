// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Web.Areas.Identity.Pages.Account.Manage;

public class PersonalDataModel(
    UserManager userManager,
    //SignInManager signInManager, 
    ILogger<PersonalDataModel> logger
    )

    : PageModel
{

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
}
