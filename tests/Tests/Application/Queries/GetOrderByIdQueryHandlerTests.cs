using Application.DTOs;
using Application.Queries;
using Application.Queries.Handlers;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Application.Queries;

public class GetOrderByIdQueryHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<ILogger<GetOrderByIdQueryHandler>> _loggerMock;
    private readonly GetOrderByIdQueryHandler _handler;

    public GetOrderByIdQueryHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _cacheMock = new Mock<IMemoryCache>();
        _loggerMock = new Mock<ILogger<GetOrderByIdQueryHandler>>();
        _handler = new GetOrderByIdQueryHandler(_orderRepositoryMock.Object, _cacheMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_CacheHit_ReturnsCachedOrder()
    {
        // Arrange
        int orderId = 1;
        var cachedOrder = new OrderDto
        {
            Id = orderId,
            CustomerName = "John Doe",
            OrderDate = DateTime.UtcNow,
            TotalAmount = 100,
            Items = []
        };

        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedOrder)).Returns(true);

        var query = new GetOrderByIdQuery(orderId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        _loggerMock.Verify(l => l.LogInformation("Cache hit for Order {OrderId}.", orderId), Times.Once);
        _orderRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CacheMiss_OrderExists_ReturnsOrderAndCachesIt()
    {
        // Arrange
        var orderId = 1;
        OrderDto? cachedOrder = null;

        var order = new Order("John Doe")
        {
            Id = orderId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = 100,
            Items = []
        };

        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedOrder)).Returns(false);
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

        var query = new GetOrderByIdQuery(orderId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        _loggerMock.Verify(l => l.LogInformation("Cache miss for Order {OrderId}, fetching from database.", orderId), Times.Once);
        _cacheMock.Verify(c => c.Set(It.IsAny<object>(), It.IsAny<OrderDto>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CacheMiss_OrderDoesNotExist_ReturnsNull()
    {
        // Arrange
        var orderId = 1;
        OrderDto? cachedOrder = null;

        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedOrder)).Returns(false);
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Order?)null);

        var query = new GetOrderByIdQuery(orderId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _loggerMock.Verify(l => l.LogInformation("Cache miss for Order {OrderId}, fetching from database.", orderId), Times.Once);
        _cacheMock.Verify(c => c.Set(It.IsAny<object>(), It.IsAny<OrderDto>(), It.IsAny<TimeSpan>()), Times.Never);
    }
}