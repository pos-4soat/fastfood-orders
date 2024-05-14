using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Application.UseCases.UpdateOrder;

public sealed record UpdateOrderResponse
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
}
