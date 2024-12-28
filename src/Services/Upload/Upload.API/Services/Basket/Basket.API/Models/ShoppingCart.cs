namespace Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

    public ShoppingCart(string userName)
    {
        this.UserName = userName;
    }

    public ShoppingCart()
    {
        
    }
}