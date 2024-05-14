using fastfood_orders.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetAllOrders;

public sealed record GetAllOrdersRequest : IRequest<Result<GetAllOrdersResponse>>;
