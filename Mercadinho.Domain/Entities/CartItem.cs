namespace Market.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!; // prop de navegação

    public CartItem() 
    {
        Id = Guid.NewGuid();
    }

    public CartItem(Guid cartId, Guid productId)
    {
        Id = Guid.NewGuid();
        CartId = cartId;
        ProductId = productId;
    }
}