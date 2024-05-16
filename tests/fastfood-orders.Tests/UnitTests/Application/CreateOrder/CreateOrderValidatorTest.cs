using fastfood_orders.Application.UseCases.CreateOrder;

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
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.OrderedItems.Clear();

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE005");
    }

    [Test]
    public void ShouldValidateUserRequirement()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.UserId = null;

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE008");
    }

    [Test]
    public void ShouldValidateUserCpf()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.UserId = "12345678908qwret";

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE009");
    }

    [Test]
    public void ShouldValidateProductIdRequirement()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.OrderedItems.Add(new OrderItens());

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE006");
    }

    [Test]
    public void ShouldValidateQuantityRequirement()
    {
        CreateOrderRequest request = _modelFakerFactory.GenerateRequest<CreateOrderRequest>();

        request.OrderedItems.Add(new OrderItens() { ProductId = 1, Quantity = 0 });

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE007");
    }
}

