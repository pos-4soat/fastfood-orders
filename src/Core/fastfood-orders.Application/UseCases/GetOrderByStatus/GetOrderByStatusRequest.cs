using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Enum;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetOrderByStatus;

public sealed record GetOrderByStatusRequest(OrderStatus Status) : IRequest<Result<GetOrderByStatusResponse>>;