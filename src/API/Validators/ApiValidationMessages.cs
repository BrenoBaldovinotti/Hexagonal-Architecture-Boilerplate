using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace API.Validators;

public static class ApiValidationMessages
{
    public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> expression)
    {
        var member = expression.Body as MemberExpression ?? ((UnaryExpression)expression.Body).Operand as MemberExpression;

        if (member == null)
        {
            throw new InvalidOperationException("Failed to retrieve member from expression.");
        }

        var property = member?.Member as PropertyInfo;

        if (property == null)
        {
            throw new InvalidOperationException("Failed to retrieve property info from member.");
        }

        var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
        return jsonPropertyNameAttribute?.Name ?? property.Name;
    }

    public static string IsRequired<T>(Expression<Func<T, object>> expression)
        => $"'{GetPropertyDisplayName(expression)}' is required.";

    public static string MustBeGreaterThan<T>(Expression<Func<T, object>> expression, int value)
        => $"'{GetPropertyDisplayName(expression)}' must be greater than {value}.";

    public static string MustBeValidEnum<T>(Expression<Func<T, object>> expression)
        => $"'{GetPropertyDisplayName(expression)}' must be a valid value.";

    public static string CannotBeNullOrEmpty<T>(Expression<Func<T, object>> expression)
        => $"'{GetPropertyDisplayName(expression)}' cannot be null or empty.";

    public static string MustBeEqualOrGreaterThan<T>(Expression<Func<T, object>> firstExpression, Expression<Func<T, object>> secondExpression)
        => $"{GetPropertyDisplayName(firstExpression)} must be equal to or greater than {GetPropertyDisplayName(secondExpression)}.";

    public static string MustBeInFormat<T>(Expression<Func<T, object>> expression, string format)
        => $"{GetPropertyDisplayName(expression)} must be in '{format}' format.";
}
