using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.GetOrderById;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Models.Response;
using Moq;
using System.Net;

namespace fastfood_orders.Tests.UnitTests.Application.GetOrderById;

public class GetOrderByIdHandlerTest : TestFixture
{
    [Test, Description("Should return order successfully")]
    public async Task ShouldReturnOrderSuccessfully()
    {
        GetOrderByIdRequest request = _modelFakerFactory.GenerateRequest<GetOrderByIdRequest>();

        OrderEntity orderedItemEntity = _modelFakerFactory.GenerateOrderEntity();

        _repositoryMock.SetupGetOrderAsync(orderedItemEntity);
        _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());

        GetOrderByIdHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object);

        Result<GetOrderByIdResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result);

        int requests = orderedItemEntity.OrderedItems.Count();

        _httpClientMock.VerifyGetProductInfo(Times.Exactly(requests));
        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyGetOrderAsync(request.OrderId, Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return no order found")]
    public async Task ShouldReturnNoOrderFound()
    {
        GetOrderByIdRequest request = _modelFakerFactory.GenerateRequest<GetOrderByIdRequest>();

        _repositoryMock.SetupGetOrderAsync(null);
        _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());

        GetOrderByIdHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object);

        Result<GetOrderByIdResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "OBE010", HttpStatusCode.BadRequest);

        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyGetOrderAsync(request.OrderId, Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }
}