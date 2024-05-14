using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Tests.UnitTests;
using Moq;

namespace fastfood_orders.Tests.Mocks;

public class OrderRepositoryMock : BaseCustomMock<IOrderRepository>
{
    public OrderRepositoryMock(TestFixture testFixture) : base(testFixture)
    {
        SetupAddOrderAsync();
        SetupEditOrderAsync();
    }

    public void SetupAddOrderAsync()
        => Setup(x => x.AddOrderAsync(It.IsAny<OrderEntity>(), default))
            .Returns(Task.CompletedTask);

    public void SetupEditOrderAsync()
        => Setup(x => x.EditOrderAsync(It.IsAny<OrderEntity>(), default))
            .Returns(Task.CompletedTask);

    public void SetupGetAllAsync(IEnumerable<OrderEntity> expectedReturn)
        => Setup(x => x.GetAllAsync(default))
            .ReturnsAsync(expectedReturn);

    public void SetupGetOrderAsync(OrderEntity expectedReturn)
        => Setup(x => x.GetOrderAsync(It.IsAny<int>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupGetOrderByStatus(IEnumerable<OrderEntity> expectedReturn)
        => Setup(x => x.GetOrderByStatus(It.IsAny<OrderStatus>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupGetPendingOrders(IEnumerable<OrderEntity> expectedReturn)
        => Setup(x => x.GetPendingOrders(default))
            .ReturnsAsync(expectedReturn);

    public void VerifyAddOrderAsync(Times? times = null)
        => Verify(x => x.AddOrderAsync(It.IsAny<OrderEntity>(), default), times ?? Times.Once());

    public void VerifyEditOrderAsync(Times? times = null)
        => Verify(x => x.EditOrderAsync(It.IsAny<OrderEntity>(), default), times ?? Times.Once());

    public void VerifyGetAllAsync(Times? times = null)
        => Verify(x => x.GetAllAsync(default), times ?? Times.Once());

    public void VerifyGetOrderAsync(int orderId, Times? times = null)
        => Verify(x => x.GetOrderAsync(orderId, default), times ?? Times.Once());

    public void VerifyGetOrderByStatus(OrderStatus orderStatus, Times? times = null)
        => Verify(x => x.GetOrderByStatus(orderStatus, default), times ?? Times.Once());

    public void VerifyGetPendingOrders(Times? times = null)
        => Verify(x => x.GetPendingOrders(default), times ?? Times.Once());
}
