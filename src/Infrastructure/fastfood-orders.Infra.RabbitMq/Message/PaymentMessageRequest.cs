using fastfood_orders.Domain.Entity;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_orders.Infra.RabbitMq.Message;

[ExcludeFromCodeCoverage]
public class PaymentMessageRequest
{
    public PaymentMessageRequest(OrderEntity orderEntity)
    {
        OrderId = orderEntity.Id;
        Amount = orderEntity.Amount.Value;
        Items = orderEntity.OrderedItems.Select(c => new Items(c));
    }

    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public IEnumerable<Items> Items { get; set; }
}

public class Items
{
    public Items(OrderedItemEntity item)
    {
        Name = item.Name;
        Quantity = item.Quantity;
        Price = item.Price;
    }

    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
