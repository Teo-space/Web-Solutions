namespace Web.Areas.Identity.Pages.Account.Manage;


public class ShowRecoveryCodesModel : PageModel
{
    [TempData]
    public string[] RecoveryCodes { get; set; } = new string[0];

    [TempData]
    public string StatusMessage { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (RecoveryCodes == null || RecoveryCodes.Length == 0)
        {
            return RedirectToPage("./TwoFactorAuthentication");
        }

        return Page();
    }
}
