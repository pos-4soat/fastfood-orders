using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Application.UseCases.CreateOrder
{
    public sealed record CreateOrderResponse
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
