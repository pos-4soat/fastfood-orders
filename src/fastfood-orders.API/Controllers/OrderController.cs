using fastfood_orders.API.Controllers.Base;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.CreateOrder;
using fastfood_orders.Application.UseCases.GetAllOrders;
using fastfood_orders.Application.UseCases.GetOrderById;
using fastfood_orders.Application.UseCases.GetOrderByStatus;
using fastfood_orders.Application.UseCases.GetPendingOrders;
using fastfood_orders.Application.UseCases.UpdateOrder;
using fastfood_orders.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace fastfood_orders.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{ver:apiVersion}/[controller]")]
public class OrderController(IMediator _mediator) : BaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create order")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<CreateOrderResponse>>))]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrderRequest, CancellationToken cancellationToken)
    {
        Result<CreateOrderResponse> result = await _mediator.Send(createOrderRequest, cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpPatch]
    [SwaggerOperation(Summary = "Update order")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<UpdateOrderResponse>>))]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest updateOrderRequest, CancellationToken cancellationToken)
    {
        Result<UpdateOrderResponse> result = await _mediator.Send(updateOrderRequest, cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("{orderId}")]
    [SwaggerOperation(Summary = "Get order by id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<GetOrderByIdResponse>>))]
    public async Task<IActionResult> GetOrderById([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        Result<GetOrderByIdResponse> result = await _mediator.Send(new GetOrderByIdRequest(orderId), cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "List all orders")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<GetAllOrdersResponse>>))]
    public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
    {
        Result<GetAllOrdersResponse> result = await _mediator.Send(new GetAllOrdersRequest(), cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("status/{status}")]
    [SwaggerOperation(Summary = "List all products")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<GetOrderByStatusResponse>>))]
    public async Task<IActionResult> GetOrderByStatus([FromRoute] OrderStatus status, CancellationToken cancellationToken)
    {
        Result<GetOrderByStatusResponse> result = await _mediator.Send(new GetOrderByStatusRequest(status), cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("pending")]
    [SwaggerOperation(Summary = "List all products")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<GetPendingOrdersResponse>>))]
    public async Task<IActionResult> GetPendingOrders(CancellationToken cancellationToken)
    {
        Result<GetPendingOrdersResponse> result = await _mediator.Send(new GetPendingOrdersRequest(), cancellationToken);
        return await GetResponseFromResult(result);
    }
}
