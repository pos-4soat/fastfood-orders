using fastfood_orders.API.Controllers;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Application.UseCases.CreateOrder;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Tests.UnitTests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TechTalk.SpecFlow;

namespace fastfood_orders.Tests.BDD;

[TestFixture]
public class CreateOrderTest : TestFixture
{
    private Mock<IMediator> _mediatorMock;
    private CreateOrderRequest _request;
    private IActionResult _result;

    [Test, Description("")]
    public async Task CreateANewOrder()
    {
        GivenIHaveAValidCreateOrderRequest();
        GivenTheRepositoryReturnsASuccessfulResult();
        await WhenIRequestAOrderCreation();
        ThenTheResultShouldBeACreatedResult();
    }

    [Given(@"I have a valid create order request")]
    public void GivenIHaveAValidCreateOrderRequest()
    {
        _request = new CreateOrderRequest()
        {
            OrderedItems =
            [
                new() {ProductId = 1, Quantity = 1 }
            ],
            UserId = "12345678909"
        };
    }

    [Given(@"the repository returns a successful result")]
    public void GivenTheRepositoryReturnsASuccessfulResult()
    {
        _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderRequest>(), default))
            .ReturnsAsync(Result<CreateOrderResponse>.Success(new CreateOrderResponse()
            {
                Id = 1,
                Status = OrderStatus.AwaitingPayment,
                TotalPrice = 10m
            }, StatusResponse.CREATED));
    }

    [When(@"I request a order creation")]
    public async Task WhenIRequestAOrderCreation()
    {
        OrderController controller = new OrderController(_mediatorMock.Object);

        _result = await controller.CreateOrder(_request, default);
    }

    [Then(@"the result should be a CreatedResult")]
    public void ThenTheResultShouldBeACreatedResult()
    {
        ObjectResult? objectResult = _result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        Response<object>? response = objectResult.Value as Response<object>;
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Status, Is.EqualTo(nameof(StatusResponse.CREATED)));

        CreateOrderResponse? body = response.Body as CreateOrderResponse;
        Assert.That(body.Id, Is.EqualTo(1));
        Assert.That(body.Status, Is.EqualTo(OrderStatus.AwaitingPayment));
    }
}
