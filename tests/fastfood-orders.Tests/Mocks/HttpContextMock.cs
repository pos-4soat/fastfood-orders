using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Tests.UnitTests;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_orders.Tests.Mocks;

public class HttpContextMock : Mock<HttpContext>
{
    public HttpContextMock()
    {
        SetupGetUserId();
        SetupGetEmail();
    }

    public void SetupGetUserId()
        => Setup(x => x.Items["UserId"])
            .Returns("12345678909");
    public void SetupGetEmail()
        => Setup(x => x.Items["Email"])
            .Returns("email@example.com");
}