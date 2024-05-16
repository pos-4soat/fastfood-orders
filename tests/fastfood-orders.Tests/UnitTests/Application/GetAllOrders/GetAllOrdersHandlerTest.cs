using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.GetAllOrders;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Models.Response;
using Moq;

namespace fastfood_orders.Tests.UnitTests.Application.GetAllOrders;

public class GetAllOrdersHandlerTest : TestFixture
{
    [Test, Description("Should return all orders successfully")]
    public async Task ShouldReturnAllOrdersSuccessfully()
    {
        GetAllOrdersRequest request = _modelFakerFactory.GenerateRequest<GetAllOrdersRequest>();

        OrderEntity orderEntity = _modelFakerFactory.GenerateOrderEntity();
        List<OrderEntity> orderList = [orderEntity];

        _repositoryMock.SetupGetAllAsync(orderList);
        _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());

        GetAllOrdersHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object);

        Result<GetAllOrdersResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result);

        int requests = orderEntity.OrderedItems.Count();

        _httpClientMock.VerifyGetProductInfo(Times.Exactly(requests));
        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyGetAllAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }
}
