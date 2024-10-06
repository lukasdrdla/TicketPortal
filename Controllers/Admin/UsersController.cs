using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketPortal.Domain.Entities;
using TicketPortal.Infrastructure.Data;
using TicketPortal.ViewModels;

namespace TicketPortal.Controllers.Admin;
[Authorize(Roles = "admin")]
public class UsersController : Controller
{    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    private readonly ApplicationDbContext _context;
    
    public UsersController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }
    
    
    /**************************************** Users ****************************************/
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }
    /****************************************************************************************/
    
    
    
    /**************************************** UserDetails ****************************************/
    public async Task<IActionResult> UserDetails(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        var model = new UserDetailsViewModel
        {
            User = user,
            Role = roles.FirstOrDefault()
        };
        
        return View(model);
    }
    /****************************************************************************************/
    

    
    /**************************************** Edit ****************************************/
    public async Task<IActionResult> UserEdit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var model = new EditUserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = roles.FirstOrDefault()
        };

        ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name", model.Role); // Dropdown pro role

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> UserEdit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name", model.Role);
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            return NotFound();
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;

        var userResult = await _userManager.UpdateAsync(user);
        if (!userResult.Succeeded)
        {
            ModelState.AddModelError("", "Chyba při aktualizaci uživatele.");
            return View(model);
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (!userRoles.Contains(model.Role))
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, model.Role);
        }

        return RedirectToAction(nameof(Users));
    }

    /****************************************************************************************/
    

    
    /**************************************** Delete ****************************************/
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Users));
    }
    
    /****************************************************************************************/
    
    
    
    
    
    /**************************************** Roles ****************************************/
    public async Task<IActionResult> Roles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }
    /****************************************************************************************/
    
    
    
    /**************************************** CreateRole ****************************************/
    public IActionResult RoleCreate()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> RoleCreate(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            ModelState.AddModelError("", "Role name is required");
            return View();
        }

        var role = new IdentityRole(roleName);
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Roles));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View();
    }
    /****************************************************************************************/
    
}