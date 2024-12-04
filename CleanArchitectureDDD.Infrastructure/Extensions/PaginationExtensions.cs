using System.Linq.Expressions;

namespace CleanArchitectureDDD.Infrastructure.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> OrderByPropertyOrField<T>(this IQueryable<T> query, string propertyOrField,
        bool ascending = true)
    {
        var elementType = typeof(T);
        var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";
        var parameterExpression = Expression.Parameter(elementType);
        var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, propertyOrField);
        
        var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);
        var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
            new[] { elementType, propertyOrFieldExpression.Type }, query.Expression, selector);

        return query.Provider.CreateQuery<T>(orderByExpression);
    }
}