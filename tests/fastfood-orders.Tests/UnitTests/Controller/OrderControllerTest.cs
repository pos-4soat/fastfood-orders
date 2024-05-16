using fastfood_orders.API.Controllers;
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
using Moq;
using System.Net;

namespace fastfood_orders.Tests.UnitTests.Controller;

public class OrderControllerTest : TestFixture
{
    [Test, Description("")]
    public async Task ShouldCreateOrder()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderRequest>(), default))
            .ReturnsAsync(Result<CreateOrderResponse>.Success(_modelFakerFactory.GenerateRequest<CreateOrderResponse>()));

        OrderController service = new(_mediatorMock.Object);

        IActionResult result = await service.CreateOrder(request, default);

        AssertExtensions.AssertResponse<CreateOrderRequest, CreateOrderResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), null);
    }

    [Test, Description("")]
    public async Task ShouldUpdateOrder()
    {
        UpdateOrderRequest request = _modelFakerFactory.GenerateRequest<UpdateOrderRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateOrderRequest>(), default))
            .ReturnsAsync(Result<UpdateOrderResponse>.Success(new UpdateOrderResponse()
            {
                Id = request.Id,
                Status = request.Status
            }));

        OrderController service = new(_mediatorMock.Object);

        IActionResult result = await service.UpdateOrder(request, default);

        AssertExtensions.AssertResponse<UpdateOrderRequest, UpdateOrderResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), request);
    }

    [Test, Description("")]
    public async Task ShouldGetOrderById()
    {
        GetOrderByIdRequest request = _modelFakerFactory.GenerateRequest<GetOrderByIdRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOrderByIdRequest>(), default))
            .ReturnsAsync(Result<GetOrderByIdResponse>.Success(new GetOrderByIdResponse()));

        OrderController service = new(_mediatorMock.Object);

        IActionResult result = await service.GetOrderById(request.OrderId, default);

        AssertExtensions.AssertResponse<GetOrderByIdRequest, GetOrderByIdResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), request);
    }

    [Test, Description("")]
    public async Task ShouldGetAllOrders()
    {
        GetAllOrdersRequest request = _modelFakerFactory.GenerateRequest<GetAllOrdersRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllOrdersRequest>(), default))
            .ReturnsAsync(Result<GetAllOrdersResponse>.Success(new GetAllOrdersResponse(_modelFakerFactory.GenerateManyRequest<GetAllOrdersOrder>())));

        OrderController service = new(_mediatorMock.Object);

        IActionResult result = await service.GetAllOrders(default);

        AssertExtensions.AssertResponse<GetAllOrdersRequest, GetAllOrdersResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), request);
    }

    [Test, Description("")]
    public async Task ShouldGetOrderByStatus()
    {
        GetOrderByStatusRequest request = _modelFakerFactory.GenerateRequest<GetOrderByStatusRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOrderByStatusRequest>(), default))
            .ReturnsAsync(Result<GetOrderByStatusResponse>.Success(new GetOrderByStatusResponse(_modelFakerFactory.GenerateManyRequest<GetOrderByStatusOrder>())));

        OrderController service = new(_mediatorMock.Object);

        IActionResult result = await service.GetOrderByStatus(request.Status, default);

        AssertExtensions.AssertResponse<GetOrderByStatusRequest, GetOrderByStatusResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), request);
    }

    [Test, Description("")]
    public async Task ShouldGetPendingOrders()
    {
        GetPendingOrdersRequest request = _modelFakerFactory.GenerateRequest<GetPendingOrdersRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetPendingOrdersRequest>(), default))
            .ReturnsAsync(Result<GetPendingOrdersResponse>.Success(new GetPendingOrdersResponse(_modelFakerFactory.GenerateManyRequest<GetPendingOrdersOrder>())));

        OrderController service = new(_mediatorMock.Object);

        IActionResult result = await service.GetPendingOrders(default);

        AssertExtensions.AssertResponse<GetPendingOrdersRequest, GetPendingOrdersResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), request);
    }


    [Test, Description("")]
    public async Task ShouldReturnProductNotFoundOnDeleteProductAsync()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderRequest>(), default))
            .ReturnsAsync(Result<CreateOrderResponse>.Failure("OBE004"));

        OrderController service = new(_mediatorMock.Object);

        IActionResult result = await service.CreateOrder(request, default);

        AssertExtensions.AssertErrorResponse(result, HttpStatusCode.BadRequest, nameof(StatusResponse.ERROR));
    }
}
