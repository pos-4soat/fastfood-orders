using fastfood_orders.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace fastfood_orders.Domain.Entity
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public decimal? Amount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.AwaitingPayment;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public IEnumerable<OrderedItemEntity> OrderedItems { get; set; }

        public bool ValidStatus(OrderStatus orderStatus)
        {
            return orderStatus == OrderStatus.Canceled || (!NewStatusIsLowerOrEqualCurrentStatus() && !NewStatusIsSkippingSteps());
            bool NewStatusIsLowerOrEqualCurrentStatus()
            => orderStatus <= Status;

            bool NewStatusIsSkippingSteps()
            => Status + 1 != orderStatus;
        }
    }
}
