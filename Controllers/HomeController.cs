using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketPortal.Interfaces;
using TicketPortal.Models;

namespace TicketPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly UserManager<AppUser> _userManager;
    private readonly IEventRepository _eventRepository;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, IEventRepository eventRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _eventRepository = eventRepository;
    }


    

    public async Task<IActionResult> Index()
    {
        
        var events = await _eventRepository.GetEvents();
        return View(events);
    }
    
    public async Task<IActionResult> Event(int id)
    {
        var eventItem = await _eventRepository.GetEvent(id);
        return View(eventItem);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Event eventItem)
    {
        if (ModelState.IsValid)
        {
            await _eventRepository.AddEvent(eventItem);
            return RedirectToAction("Index");
        }
        return View(eventItem);
    }

    
  
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var eventItem = await _eventRepository.GetEvent(id);
        if (eventItem == null)
        {
            return NotFound(); 
        }
            
        await _eventRepository.DeleteEvent(eventItem);
        return RedirectToAction("Index");
    }
    
    
    
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}