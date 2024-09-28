using TicketPortal.Models;

namespace TicketPortal.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetEvents();
    Task<Event> GetEvent(int id);
    
    Task AddEvent(Event eventItem);
    
    Task DeleteEvent(Event eventItem);
    
}