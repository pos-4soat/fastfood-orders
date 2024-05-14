using fastfood_orders.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetPendingOrders;

public sealed record class GetPendingOrdersRequest : IRequest<Result<GetPendingOrdersResponse>>
{
}
