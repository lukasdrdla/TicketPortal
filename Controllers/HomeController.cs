using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketPortal.Application.Interfaces;
using TicketPortal.Domain.Entities;
using TicketPortal.ViewModels;

namespace TicketPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IEventRepository _eventRepository;
    private readonly ITicketRepository _ticketRepository;
    
    public HomeController(ILogger<HomeController> logger, IEventRepository eventRepository, ITicketRepository ticketRepository)
    {
        _logger = logger;
        _eventRepository = eventRepository;
        _ticketRepository = ticketRepository;
    }

    
    public async Task<IActionResult> Index(string search)
    {
        var events = await _eventRepository.GetEvents();
        if (!string.IsNullOrEmpty(search))
        {
            events = events.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                                       x.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return View(events);
    }
    
    public async Task<IActionResult> EventDetail(int id)
    {
        var eventItem = await _eventRepository.GetEvent(id);
        var ticket = await _ticketRepository.GetTicketByEventId(id);
        
        var viewModel = new EventDetailViewModel
        {
            Event = eventItem,
            Ticket = ticket
        };
        
        
        
        
        
        return View(viewModel);
    }
    
    public IActionResult ConfirmOrder()
    {
        return View();
    }
    

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}