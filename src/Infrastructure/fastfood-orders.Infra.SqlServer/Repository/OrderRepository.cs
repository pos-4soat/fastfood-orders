using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Infra.SqlServer.Context;
using Microsoft.EntityFrameworkCore;

namespace fastfood_orders.Infra.SqlServer.Repository
{
    public class OrderRepository : IOrderRepository
    {
        protected DbSet<OrderEntity> Data { get; }
        protected ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            Data = _context.Set<OrderEntity>();
        }

        public async Task AddOrderAsync(OrderEntity order, CancellationToken cancellationToken)
        {
            await Data.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            order = await Data.Include(x => x.OrderedItems).FirstAsync(x => x.Id == order.Id, cancellationToken: cancellationToken);
        }

        public async Task EditOrderAsync(OrderEntity order, CancellationToken cancellationToken)
        {
            Data.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken)
             => await Data
                .Include(x => x.OrderedItems)
                .OrderBy(x => x.Status)
                .ToListAsync(cancellationToken);

        public async Task<OrderEntity?> GetOrderAsync(int orderId, CancellationToken cancellationToken)
            => await Data.Include(x => x.OrderedItems).FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken: cancellationToken);

        public async Task<IEnumerable<OrderEntity>> GetOrderByStatus(OrderStatus status, CancellationToken cancellationToken)
            => await Data
                .Include(x => x.OrderedItems)
                .Where(x => x.Status == status)
                .ToListAsync(cancellationToken: cancellationToken);

        public async Task<IEnumerable<OrderEntity>> GetPendingOrders(CancellationToken cancellationToken)
            => await Data
                .Where(x => x.Status == OrderStatus.Received
                         || x.Status == OrderStatus.InPreparation
                         || x.Status == OrderStatus.Ready)
                .OrderByDescending(x => x.Status)
                .ThenBy(x => x.CreationDate)
                .Include(x => x.OrderedItems)
                .ToListAsync(cancellationToken);
    }
}
