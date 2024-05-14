using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Tests.UnitTests;
using MediatR;
using Moq;

namespace fastfood_orders.Tests.Mocks;

public class MediatorMock<TRequest, TResponse>(TestFixture testFixture) : BaseCustomMock<IMediator>(testFixture) where TRequest : notnull
{
    public void SetupSendAsync(TResponse response, StatusResponse statusResponse)
        => Setup(x => x.Send(It.IsAny<TRequest>(), default))
            .ReturnsAsync(Result<TResponse>.Success(response, statusResponse));
}

