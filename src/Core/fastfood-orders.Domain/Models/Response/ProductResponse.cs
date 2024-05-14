namespace fastfood_orders.Domain.Models.Response
{
    public class ProductResponse
    {
        public ProductData body { get; set; }
    }

    public class ProductData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
