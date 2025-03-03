using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class AccountController: Controller

{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var identityUser = new IdentityUser
        {
            UserName = model.Username,
            Email = model.Email
        };
        var identityResult = await _userManager.CreateAsync(identityUser, model.Password);

        if (identityResult.Succeeded)
        {
            // Assign this user the user role
            var roleIdentityResult = await _userManager.AddToRoleAsync(identityUser, "User");

            if (roleIdentityResult.Succeeded)
            {
                return RedirectToAction("Register");
            }
        }
        
        return View("");
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

        if (signInResult != null && signInResult.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}