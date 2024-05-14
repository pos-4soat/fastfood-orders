using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Domain.Contracts.Repository
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(OrderEntity order, CancellationToken cancellationToken);
        Task EditOrderAsync(OrderEntity order, CancellationToken cancellationToken);
        Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<OrderEntity?> GetOrderAsync(int orderId, CancellationToken cancellationToken);
        Task<IEnumerable<OrderEntity>> GetOrderByStatus(OrderStatus status, CancellationToken cancellationToken);
        Task<IEnumerable<OrderEntity>> GetPendingOrders(CancellationToken cancellationToken);
    }
}
