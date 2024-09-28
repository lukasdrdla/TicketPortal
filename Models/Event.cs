namespace TicketPortal.Models;

public class Event
{
    public int Id { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public decimal Price { get; set; }
    public int MaxCapacity { get; set; }
    public int TicketsSold { get; set; }
    public string EventType { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Address { get; set; }
    public string Organizer { get; set; }
}
