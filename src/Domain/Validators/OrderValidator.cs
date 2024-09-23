using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.CustomerName)
                .NotEmpty().WithMessage(DomainValidationMessages.IsRequired<Order>(o => o.CustomerName));

            RuleFor(order => order.Items)
                .NotEmpty().WithMessage(DomainValidationMessages.IsRequired<Order>(o => o.Items));
        }
    }
}
