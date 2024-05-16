using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.GetOrderByStatus;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Models.Response;
using Moq;

namespace fastfood_orders.Tests.UnitTests.Application.GetOrderByStatus;

public class GetOrderByStatusHandlerTest : TestFixture
{
    [Test, Description("Should return orders by status successfully")]
    public async Task ShouldReturnOrdersByStatusSuccessfully()
    {
        GetOrderByStatusRequest request = _modelFakerFactory.GenerateRequest<GetOrderByStatusRequest>();

        OrderEntity orderEntity = _modelFakerFactory.GenerateOrderEntity();
        List<OrderEntity> orderList = [orderEntity];

        _repositoryMock.SetupGetOrderByStatus(orderList);
        _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());

        GetOrderByStatusHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object);

        Result<GetOrderByStatusResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result);

        int requests = orderEntity.OrderedItems.Count();

        _httpClientMock.VerifyGetProductInfo(Times.Exactly(requests));
        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyGetOrderByStatus(request.Status, Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }
}
