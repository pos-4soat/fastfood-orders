using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.GetPendingOrders;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Models.Response;
using Moq;

namespace fastfood_orders.Tests.UnitTests.Application.GetPendingOrders;

public class GetPendingOrdersHandlerTest : TestFixture
{
    [Test, Description("Should return pending orders successfully")]
    public async Task ShouldReturnPendingOrdersSuccessfully()
    {
        GetPendingOrdersRequest request = _modelFakerFactory.GenerateRequest<GetPendingOrdersRequest>();

        OrderEntity orderEntity = _modelFakerFactory.GenerateOrderEntity();
        List<OrderEntity> orderList = [orderEntity];

        _repositoryMock.SetupGetPendingOrders(orderList);
        _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());

        GetPendingOrdersHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object);

        Result<GetPendingOrdersResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result);

        int requests = orderEntity.OrderedItems.Count();

        _httpClientMock.VerifyGetProductInfo(Times.Exactly(requests));
        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyGetPendingOrders(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }
}
