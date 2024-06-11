using fastfood_orders.Domain.Entity;

namespace fastfood_orders.Domain.Contracts.RabbitMq;

public interface IRabbitService
{
    Task<string> Publish(OrderEntity orderEntity);
    void StartConsuming();
    void Dispose();
}
