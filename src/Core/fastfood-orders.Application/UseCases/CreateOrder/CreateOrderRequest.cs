using fastfood_orders.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_orders.Application.UseCases.CreateOrder
{
    public class CreateOrderRequest : IRequest<Result<CreateOrderResponse>>
    {
        public IList<OrderItens> OrderedItems { get; set; }
        public string? UserId { get; set; }
    }

    public sealed record OrderItens
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
