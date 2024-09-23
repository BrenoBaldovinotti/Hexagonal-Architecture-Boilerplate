using Domain.Entities;
using FluentValidation;

namespace Domain.Validators;

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(item => item.ProductName)
            .NotEmpty().WithMessage(DomainValidationMessages.IsRequired<OrderItem>(o => o.ProductName));

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage(DomainValidationMessages.MustBeGreaterThan<OrderItem>(o => o.UnitPrice, 0));

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage(DomainValidationMessages.MustBeGreaterThan<OrderItem>(o => o.Quantity, 0));
    }
}
