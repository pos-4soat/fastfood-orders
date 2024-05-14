using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Models.Response;
using fastfood_orders.Tests.UnitTests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_orders.Tests.Mocks;

public class ProductHttpClientMock : BaseCustomMock<IProductHttpClient>
{
    public ProductHttpClientMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupGetProductInfo(ProductData expectedReturn)
        => Setup(x => x.GetProductInfo(It.IsAny<int>(), default))
            .ReturnsAsync(expectedReturn);

    public void VerifyGetProductInfo(Times? times = null)
        => Verify(x => x.GetProductInfo(It.IsAny<int>(), default), times ?? Times.Once());
}
