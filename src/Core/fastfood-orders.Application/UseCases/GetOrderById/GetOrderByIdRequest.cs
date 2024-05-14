using fastfood_orders.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetOrderById;

public sealed record GetOrderByIdRequest(int OrderId) : IRequest<Result<GetOrderByIdResponse>>;