using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Enum;
using MediatR;

namespace fastfood_orders.Application.UseCases.UpdateOrder;

public sealed record UpdateOrderRequest(int Id, OrderStatus Status)
    : IRequest<Result<UpdateOrderResponse>>;
