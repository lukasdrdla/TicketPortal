using TicketPortal.Domain.Entities;

namespace TicketPortal.ViewModels;

public class UserDetailsViewModel
{
    public AppUser User { get; set; }
    public string Role { get; set; }
    
}