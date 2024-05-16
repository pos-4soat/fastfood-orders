using fastfood_orders.Application.UseCases.UpdateOrder;
using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Tests.UnitTests.Application.UpdateOrder;

public class UpdateOrderValidatorTest : TestFixture
{
    private UpdateOrderValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateOrderValidator();
    }

    [Test]
    public void ShouldValidateOrderValue()
    {
        UpdateOrderRequest request = new UpdateOrderRequest(-1, OrderStatus.Ready);

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE012");
    }

    [Test]
    public void ShouldValidateStatusValue()
    {
        UpdateOrderRequest request = new UpdateOrderRequest(1, (OrderStatus)99);

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "OBE013");
    }
}
