using fastfood_orders.Application.UseCases.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_orders.Tests.UnitTests.Application.CreateOrder;

public class CreateOrderValidatorTest : TestFixture
{
    private CreateOrderValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateOrderValidator();
    }

    [Test]
    public void ShouldValidateRequirement()
    {
        var request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.OrderedItems.Clear();

        var result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE005");
    }

    [Test]
    public void ShouldValidateUserRequirement()
    {
        var request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.UserId = null;

        var result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE008");
    }

    [Test]
    public void ShouldValidateUserCpf()
    {
        var request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.UserId = "12345678908qwret";

        var result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE009");
    }

    [Test]
    public void ShouldValidateProductIdRequirement()
    {
        var request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.OrderedItems.Add(new OrderItens());

        var result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE006");
    }

    [Test]
    public void ShouldValidateQuantityRequirement()
    {
        var request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.OrderedItems.Add(new OrderItens() { ProductId = 1, Quantity = 0});

        var result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE007");
    }
}

