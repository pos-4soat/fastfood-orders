using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Application.UseCases.GetAllOrders;

public sealed record GetAllOrdersResponse
{
    public GetAllOrdersResponse(IEnumerable<GetAllOrdersOrder> orders)
    {
        Orders = orders;
    }

    public IEnumerable<GetAllOrdersOrder> Orders { get; set; }
}

public sealed record GetAllOrdersOrder
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public string? UserId { get; set; }
    public IEnumerable<GetAllOrdersProductData> Items { get; set; }
}

public sealed record GetAllOrdersProductData
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
