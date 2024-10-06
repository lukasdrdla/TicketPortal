using TicketPortal.Domain.Entities;

namespace TicketPortal.ViewModels;

public class EventDetailViewModel
{
    public Event Event { get; set; }
    public Ticket Ticket { get; set; }
}