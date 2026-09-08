namespace Market.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Barcode { get; private set; } = string.Empty;
        public decimal CostPrice { get; private set; }
        public decimal SellingPrice { get; private set; }
        public int Quantity { get; private set; }
        public int QuantityByBatch { get; private set; }
        public bool Active { get; private set; }
        public CartItem? Item { get; private set; }
        public Sector Sector { get; private set; } = null!;
        public IEnumerable<Batch> ProductBatches { get; private set; } = new List<Batch>();

        public Product()
        {
            Id = Guid.NewGuid();
            Active = false;
        }

        public Product(string name, string barcode, decimal costPrice, decimal sellingPrice)
        {
            Id = Guid.NewGuid();
            Name = name;
            Barcode = barcode;
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
            Active = false;
            QuantityByBatch = 0;
            Quantity = 0;
            ProductBatches = new List<Batch>();
        }

        public void UpdateDetails(string name, string barcode)
        {
            Name = name;
            Barcode = barcode;
        }

        public void UpdatePrices(decimal costPrice, decimal sellingPrice)
        {
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
        }

        public void Activate()
        {
            Active = true;
        }

        public void Inactivate()
        {
            Active = false;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount < 0) throw new ArgumentException("Amount cannot be negative");
            Quantity += amount;
        }

        public void DecreaseQuantity(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }
            if (Quantity - amount < 0)
            {
                throw new InvalidOperationException("Insufficient quantity");
            }

            Quantity -= amount;
        }

        public void SetQuantityByBatch(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }
                QuantityByBatch = quantity;
        }

        public void AssignSector(Sector sector)
        {
            Sector = sector;
            if (Sector == null)
            {
                throw new ArgumentNullException(nameof(sector));
            }
        }
    }
}