using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using FluentValidation;
using MediatR;

namespace Application.Commands.Handlers;

public class PlaceOrderCommandHandler(
    IOrderRepository orderRepository,
    IValidator<Order> orderValidator,
    OrderService orderService) : IRequestHandler<PlaceOrderCommand, int>
{
    public async Task<int> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(request.CustomerName);
        foreach (var item in request.Items)
        {
            order.AddItem(new OrderItem(item.ProductName, item.UnitPrice, item.Quantity));
        }

        var validationResult = orderValidator.Validate(order);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        orderService.PlaceOrder(order);

        await orderRepository.AddAsync(order);

        return order.Id;
    }
}
