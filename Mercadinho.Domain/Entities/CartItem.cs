namespace Market.Domain.Entities;

public class CartItem
{
    public Guid CartItemId { get; set; }
    public Guid CartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Cart Cart { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!; // prop de navegação

    public CartItem() 
    {
        CartItemId = Guid.NewGuid();
    }

    public CartItem(Guid cartId, Guid productId, int quantity, decimal unitPrice)
    {
        CartItemId = Guid.NewGuid();
        CartId = cartId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}