using IdentityService.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Route("[controller]")]
[ApiController]
public class IdentityController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(AuthModel authModel)
    {
        authModel.ReturnUrl ??= Url.Content("~/");

        var externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(authModel.Email, authModel.Password,
                authModel.RememberMe.GetValueOrDefault(false), lockoutOnFailure: false);
            if (result.Succeeded)
            {
                //   _logger.LogInformation("User logged in.");
                return LocalRedirect(authModel.ReturnUrl);
            }

            if (result.IsLockedOut)
            {
                //      _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest('1');
            }
        }

        // If we got this far, something failed, redisplay form
        return BadRequest('2');
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register(AuthModel authModel)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser {Email = authModel.Email, UserName = authModel.Email};

            var registerResult = await _userManager.CreateAsync(user, authModel.Password);

            if (registerResult.Succeeded)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(authModel.Email, authModel.Password,
                    false, lockoutOnFailure: false);

                return LocalRedirect(authModel.ReturnUrl);
            }

            return BadRequest('3');
        }

        // If we got this far, something failed, redisplay form
        return BadRequest('4');
    }
}