using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketPortal.Application.Interfaces;
using TicketPortal.Domain.Entities;
using TicketPortal.Infrastructure.Data;


namespace TicketPortal.Controllers.Admin;

[Authorize(Roles = "admin")]
public class EventsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IEventRepository _eventRepository;
    
    public EventsController(ApplicationDbContext context, IEventRepository eventRepository)
    {
        _context = context;
        _eventRepository = eventRepository;
    }

    /**************************************** Events ****************************************/
    public async Task<IActionResult> Events()
    {
        var events = await _eventRepository.GetEvents();
        return View(events);
    }
    
    /****************************************************************************************/
    
    /**************************************** EventDetails ****************************************/
    public async Task<IActionResult> EventDetails(int id)
    {
        var eventItem = await _eventRepository.GetEvent(id);
        if (eventItem == null)
        {
            return NotFound();
        }
        return View(eventItem);
    }
    /****************************************************************************************/
    
    /**************************************** Edit ****************************************/
    public async Task<IActionResult> EventEdit(int id)
    {
        var eventItem = await _eventRepository.GetEvent(id);
        if (eventItem == null)
        {
            return NotFound();
        }
        return View(eventItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EventEdit(Event eventItem)
    {
        await _eventRepository.UpdateEvent(eventItem, eventItem.Id);
        
        
        return RedirectToAction("Events");
    }

    /****************************************************************************************/
    
    
    
    /**************************************** Create ****************************************/
    public IActionResult EventCreate()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> EventCreate(Event eventItem)
    {
        if (!ModelState.IsValid)
        {
            return View(eventItem);
        }
        await _eventRepository.AddEvent(eventItem);
        return RedirectToAction("Events");
    }
    /****************************************************************************************/
    
    /**************************************** Delete ****************************************/
    [HttpPost]
    public async Task<IActionResult> EventDelete(int id)
    {
        var eventItem = await _eventRepository.GetEvent(id);
        if (eventItem == null)
        {
            return NotFound();
        }
        await _eventRepository.DeleteEvent(eventItem);
        return RedirectToAction("Events");
    }
    /****************************************************************************************/
}