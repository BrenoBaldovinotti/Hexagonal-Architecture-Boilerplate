﻿using Domain.Entities;
using Domain.Validators;
using FluentValidation;

namespace Domain.Services;

public class OrderService(IValidator<Order> orderValidator, IValidator<OrderItem> orderItemValidator)
{
    public void PlaceOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var validationResult = orderValidator.Validate(order);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        foreach (var item in order.Items)
        {
            var itemValidationResult = orderItemValidator.Validate(item);
            if (!itemValidationResult.IsValid)
            {
                throw new ValidationException(itemValidationResult.Errors);
            }
        }

        // Proceed with the rest of the business logic
        // e.g., persisting the order, sending notifications, etc. 
    }
}
