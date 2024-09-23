using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Validators;

public static class DomainValidationMessages
{
    public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
    {
        var member = expression.Body as MemberExpression ?? ((UnaryExpression)expression.Body).Operand as MemberExpression;

        if (member == null)
        {
            throw new InvalidOperationException("Failed to retrieve member from expression.");
        }

        var property = member?.Member as PropertyInfo;
        return property?.Name ?? throw new InvalidOperationException("Failed to retrieve property info.");
    }

    public static string IsRequired<T>(Expression<Func<T, object>> expression)
        => $"'{GetPropertyName(expression)}' is required.";

    public static string MustBeGreaterThan<T>(Expression<Func<T, object>> expression, int value)
        => $"'{GetPropertyName(expression)}' must be greater than {value}.";

    public static string CannotBeNullOrEmpty<T>(Expression<Func<T, object>> expression)
        => $"'{GetPropertyName(expression)}' cannot be null or empty.";

    // Additional domain-specific validation messages
}
