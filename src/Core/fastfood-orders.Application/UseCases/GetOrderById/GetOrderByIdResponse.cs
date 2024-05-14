using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Application.UseCases.GetOrderById;

public sealed record GetOrderByIdResponse
{
    public GetOrderByIdResponse()
    {

    }

    public GetOrderByIdResponse(int id, OrderStatus status, IEnumerable<GetOrderByIdProductData> items)
    {
        Id = id;
        Status = status;
        Items = items;
    }

    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public string? UserId { get; set; }
    public decimal Amount { get; set; }
    public IEnumerable<GetOrderByIdProductData> Items { get; set; }
}

public sealed record GetOrderByIdProductData
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
