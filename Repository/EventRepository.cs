using Microsoft.EntityFrameworkCore;
using TicketPortal.Data;
using TicketPortal.Interfaces;
using TicketPortal.Models;

namespace TicketPortal.Repository;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;
    
    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Event>> GetEvents()
    {
        var events = await _context.Events.ToListAsync();
        return events;
    }

    public async Task<Event> GetEvent(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        return eventItem;
    }

    public async Task AddEvent(Event eventItem)
    {
        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();
        
    }
    
    public async Task DeleteEvent(Event eventItem)
    {
        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
    }
}