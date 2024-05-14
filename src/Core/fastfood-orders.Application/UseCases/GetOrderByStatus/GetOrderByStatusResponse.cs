using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Application.UseCases.GetOrderByStatus;

public sealed record GetOrderByStatusResponse
{
    public GetOrderByStatusResponse(IEnumerable<GetOrderByStatusOrder> orders)
    {
        Orders = orders;
    }

    public IEnumerable<GetOrderByStatusOrder> Orders { get; set; }
}

public sealed record GetOrderByStatusOrder
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<GetOrderByStatusProductData> Items { get; set; }
}

public sealed record GetOrderByStatusProductData
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
