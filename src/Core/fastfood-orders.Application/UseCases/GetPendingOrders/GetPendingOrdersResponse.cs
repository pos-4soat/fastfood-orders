using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Application.UseCases.GetPendingOrders;

public sealed record GetPendingOrdersResponse
{
    public GetPendingOrdersResponse(IEnumerable<GetPendingOrdersOrder> orders)
    {
        Orders = orders;
    }

    public IEnumerable<GetPendingOrdersOrder> Orders { get; set; }
}

public sealed record GetPendingOrdersOrder
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<GetPendingOrdersProductData> Items { get; set; }
}

public sealed record GetPendingOrdersProductData
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
