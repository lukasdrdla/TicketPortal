using TicketPortal.Domain.Entities;

namespace TicketPortal.ViewModels;

public class ShoppingCartViewModel
{
    public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    public decimal ShoppingCartTotal { get; set; }
    public int? TotalQuantity { get; set; }
    
}