﻿using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.CreateOrder;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Domain.Models.Response;
using Moq;
using System.Net;

namespace fastfood_orders.Tests.UnitTests.Application.CreateOrder;

public class CreateOrderHandlerTest : TestFixture
{
    [Test, Description("Should return order created successfully")]
    public async Task ShouldCreateOrderAsync()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());
        _rabbitMock.SetupPublish(Faker.Lorem.Word());

        CreateOrderHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object, _rabbitMock.Object);

        Result<CreateOrderResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.Created);

        Assert.That(result.Value.Status, Is.EqualTo(OrderStatus.AwaitingPayment));

        int requests = request.OrderedItems.Count;

        _httpClientMock.VerifyGetProductInfo(Times.Exactly(requests));
        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyAddOrderAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _rabbitMock.VerifyPublish(Times.Once());
        _rabbitMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return product not valid")]
    public async Task ShouldReturnProductNotValid()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        _httpClientMock.SetupGetProductInfo(null);

        CreateOrderHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object, _rabbitMock.Object);

        Result<CreateOrderResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "OBE004", HttpStatusCode.BadRequest);

        _httpClientMock.VerifyGetProductInfo(Times.Once());
        _httpClientMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
        _rabbitMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return failed order creation")]
    public async Task ShouldReturnFailedCreation()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        _httpClientMock.SetupGetProductInfo(_modelFakerFactory.GenerateRequest<ProductData>());
        _rabbitMock.SetupPublish(string.Empty);

        CreateOrderHandler service = new(_mapper, _repositoryMock.Object, _httpClientMock.Object, _rabbitMock.Object);

        Result<CreateOrderResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "OBE015", HttpStatusCode.BadRequest);

        int requests = request.OrderedItems.Count;

        _httpClientMock.VerifyGetProductInfo(Times.Exactly(requests));
        _httpClientMock.VerifyNoOtherCalls();
        _rabbitMock.VerifyPublish(Times.Once());
        _rabbitMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyEditOrderAsync(Times.Once());
        _repositoryMock.VerifyAddOrderAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
    }
}