using System.ComponentModel.DataAnnotations;

namespace TicketPortal.ViewModels;

public class EditUserViewModel
{
    public string Id { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Role { get; set; } // pro výběr role
    
}