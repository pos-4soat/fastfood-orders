using fastfood_orders.Domain.Entity;

namespace fastfood_orders.Domain.Contracts.RabbitMq;

public interface IProducerService
{
    Task<string> Publish(OrderEntity orderEntity);
}
