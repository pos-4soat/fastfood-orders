using System.ComponentModel.DataAnnotations;

namespace fastfood_orders.Domain.Entity
{
    public class OrderedItemEntity
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public OrderEntity Order { get; set; }
    }
}
