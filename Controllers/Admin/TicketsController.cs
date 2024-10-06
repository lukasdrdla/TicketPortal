using Microsoft.AspNetCore.Mvc;
using TicketPortal.Application.Interfaces;

namespace TicketPortal.Controllers.Admin;

public class TicketsController : Controller
{
    private readonly ITicketRepository _ticketRepository;
    
    public TicketsController(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }
    
    public async Task<IActionResult> Index()
    {
        var tickets = await _ticketRepository.GetAllTickets(); 
        return View(tickets);
    }
}