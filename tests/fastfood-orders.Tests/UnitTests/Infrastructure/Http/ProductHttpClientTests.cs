using fastfood_orders.Infra.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_orders.Tests.UnitTests.Infrastructure.Http;

public class ProductHttpClientTests : TestFixture
{
    [Test]
    public async Task ShouldFailReadFromJsonAsyncOnGetProductInfo()
    {
        ProductHttpClient client = new("https://webhook.d3fkon1.com/728ed6f6-8233-41a9-8709-da242467936b/");

        var result = await client.GetProductInfo(Faker.Random.Int(), CancellationToken.None);

        Assert.That(result, Is.Null);
    }
}