using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.CreateOrder;
using fastfood_orders.Application.UseCases.GetAllOrders;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Domain.Models.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_orders.Tests.UnitTests.Application.GetAllOrders;

public class GetAllOrdersHandlerTest : TestFixture
{
    //[Test, Description("Should return order created successfully")]
    //public async Task ShouldCreateOrderAsync()
    //{
    //    GetAllOrdersRequest request = _modelFakerFactory.GenerateRequest<GetAllOrdersRequest>();

    //    var orderedItemEntity = _modelFakerFactory.GenerateManyRequest<OrderedItemEntity>();
    //    var orderEntity = _modelFakerFactory.GenerateRequest<OrderEntity>();
    //    _repositoryMock.SetupGetAllAsync(orderEntities);
    //    _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());

    //    GetAllOrdersHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object);

    //    Result<GetAllOrdersResponse> result = await service.Handle(request, default);

    //    AssertExtensions.ResultIsSuccess(result);

    //    int requests = orderEntities.Count();

    //    _httpClientMock.VerifyGetProductInfo(Times.Exactly(requests));
    //    _httpClientMock.VerifyNoOtherCalls();
    //    _repositoryMock.VerifyGetAllAsync(Times.Once());
    //    _repositoryMock.VerifyNoOtherCalls();
    //}

    [Test, Description("Should return product not valid")]
    public async Task ShouldReturnProductNotValid()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        _httpClientMock.SetupGetProductInfo(null);

        CreateOrderHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object);

        Result<CreateOrderResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "OBE004", HttpStatusCode.BadRequest);

        _httpClientMock.VerifyGetProductInfo(Times.Once());
        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
    }
}
