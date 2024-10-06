using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketPortal.Domain.Entities;
using TicketPortal.ViewModels;

namespace TicketPortal.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    /**************************************** Register ****************************************/
    public IActionResult Register()
    {
        RegisterViewModel registerViewModel = new RegisterViewModel();
        return View(registerViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (ModelState.IsValid)
        {
            AppUser user = new AppUser
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                PhoneNumber = registerViewModel.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(registerViewModel);
    }
    /****************************************************************************************/
    
    
    /**************************************** Login ****************************************/
    public IActionResult Login()
    {
        LoginViewModel loginViewModel = new LoginViewModel();
        return View(loginViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid login attempt");
        }
        return View(loginViewModel);
    }
    /****************************************************************************************/
    
    /**************************************** Logout ****************************************/
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    /****************************************************************************************/
    
    /**************************************** UserProfile ****************************************/
    [Authorize]
    public async Task<IActionResult> UserProfile()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            Redirect("/Account/Login");
        }
        return View(currentUser);
    }
    /****************************************************************************************/
    
}