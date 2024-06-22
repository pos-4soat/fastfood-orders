using fastfood_orders.Application.Shared.BaseResponse;
using MediatR;
using System.Text.Json.Serialization;

namespace fastfood_orders.Application.UseCases.CreateOrder
{
    public class CreateOrderRequest : IRequest<Result<CreateOrderResponse>>
    {
        public IList<OrderItens> OrderedItems { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }

        [JsonIgnore]
        public string? Email { get; set; }
    }

    public sealed record OrderItens
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
