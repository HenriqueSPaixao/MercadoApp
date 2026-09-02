namespace Market.Domain.Entities;

public class CartItem
{
    public int Id { get; set; }

    // Chave estrangeira e navegação do Carrinho
    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    // Chave estrangeira e navegação do Produto
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; } // Congelado no momento da adição

    // Propriedade calculada
    public decimal Subtotal
    {
        get
        {
            return Quantity * UnitPrice;
        }
    }
    //public decimal Subtotal => Quantity * UnitPrice;

    // Construtor vazio exigido pelo EF Core
    protected CartItem() { }

    // Construtor de negócio (garante integridade na criação)
    public CartItem(int productId, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");
        if (unitPrice < 0)
            throw new ArgumentException("O preço unitário não pode ser negativo.");

        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public void QuantityUpdate(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        Quantity = newQuantity;
    }

    public void IncreaseQuantity(int aditionalQuantity)
    {
        if (aditionalQuantity <= 0)
            throw new ArgumentException("A quantidade a incrementar deve ser maior que zero.");

        Quantity += aditionalQuantity;
    }
}