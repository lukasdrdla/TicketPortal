using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketPortal.Data;
using TicketPortal.Domain.Entities;
using TicketPortal.ViewModels;
using TicketPortal.Infrastructure.Data;

namespace TicketPortal.Controllers;

public class ShoppingCartController : Controller
{
    private readonly ApplicationDbContext _context;
    private List<ShoppingCartItem> _shoppingCartItems;
    
    public ShoppingCartController(ApplicationDbContext context)
    {
        _context = context;
        _shoppingCartItems = new List<ShoppingCartItem>();
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult CartItemCount()
    {
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("cart") ?? new List<ShoppingCartItem>();
        var itemCount = cartItems.Sum(x => x.Quantity);
        return PartialView("CartSummary", itemCount);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("cart") ?? new List<ShoppingCartItem>();
        ViewBag.CartItemCount = cartItems.Sum(x => x.Quantity); // Celkový počet položek
        base.OnActionExecuting(context);
    }
    public IActionResult AddToCart(int ticketId)
    {
        
        var ticketToAdd = _context.Tickets.FirstOrDefault(x => x.Id == ticketId);
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("cart") ?? new List<ShoppingCartItem>();
        var existingCartItem = cartItems.FirstOrDefault(x => x.Ticket.Id == ticketId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity++;
        }
        else
        {
            cartItems.Add(new ShoppingCartItem
            {
                Ticket = ticketToAdd,
                Quantity = 1
            });
        }
        
        HttpContext.Session.Set("cart", cartItems);
        
        
        
        return RedirectToAction("viewCart");



    }
    
    public IActionResult ViewCart()
    {
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("cart") ?? new List<ShoppingCartItem>();
        
        var cartViewModel = new ShoppingCartViewModel
        {
            ShoppingCartItems = cartItems,
            ShoppingCartTotal = cartItems.Sum(x => x.Ticket.Price * x.Quantity),
        };
        
        return View(cartViewModel);
    }
}