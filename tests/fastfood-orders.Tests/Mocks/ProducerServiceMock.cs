using fastfood_orders.Domain.Contracts.RabbitMq;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Tests.UnitTests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_orders.Tests.Mocks;

public class ProducerServiceMock(TestFixture testFixture) : BaseCustomMock<IRabbitService>(testFixture)
{
    public void SetupPublish(string expectedReturn)
        => Setup(x => x.Publish(It.IsAny<OrderEntity>()))
            .ReturnsAsync(expectedReturn);

    public void VerifyPublish(Times? times = null)
        => Verify(x => x.Publish(It.IsAny<OrderEntity>()), times ?? Times.Once());
}
