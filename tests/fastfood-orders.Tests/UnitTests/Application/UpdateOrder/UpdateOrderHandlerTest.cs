using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.UpdateOrder;
using fastfood_orders.Domain.Entity;
using Moq;
using System.Net;

namespace fastfood_orders.Tests.UnitTests.Application.UpdateOrder;

public class UpdateOrderHandlerTest : TestFixture
{
    [Test, Description("Should update order successfully")]
    public async Task ShouldUpdateOrderSuccessfully()
    {
        UpdateOrderRequest request = _modelFakerFactory.GenerateRequest<UpdateOrderRequest>();

        OrderEntity orderEntity = _modelFakerFactory.GenerateOrderEntityStatus(request.Status);

        _repositoryMock.SetupGetOrderAsync(orderEntity);

        UpdateOrderHandler service = new(_mapper, _repositoryMock.Object);

        Result<UpdateOrderResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result);

        _repositoryMock.VerifyGetOrderAsync(request.Id, Times.Once());
        _repositoryMock.VerifyEditOrderAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return no rder found")]
    public async Task ShouldReturnNoOrderFound()
    {
        UpdateOrderRequest request = _modelFakerFactory.GenerateRequest<UpdateOrderRequest>();

        _repositoryMock.SetupGetOrderAsync(null);

        UpdateOrderHandler service = new(_mapper, _repositoryMock.Object);

        Result<UpdateOrderResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "OBE010", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyGetOrderAsync(request.Id, Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return invalid status")]
    public async Task ShouldReturnInvalidStatus()
    {
        UpdateOrderRequest request = _modelFakerFactory.GenerateRequest<UpdateOrderRequest>();

        OrderEntity orderEntity = _modelFakerFactory.GenerateOrderEntityStatus(request.Status);
        orderEntity.Status = request.Status + 1;

        _repositoryMock.SetupGetOrderAsync(orderEntity);

        UpdateOrderHandler service = new(_mapper, _repositoryMock.Object);

        Result<UpdateOrderResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "OBE014", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyGetOrderAsync(request.Id, Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }
}

