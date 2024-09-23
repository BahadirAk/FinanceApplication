using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApplication.Core.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> IncludeNested<T>(this IQueryable<T> query, string includes) where T : class
    {
        if (string.IsNullOrEmpty(includes))
            return query;
        
        var includeArray = includes.Split(',');

        foreach (var include in includeArray)
        {
            query = ApplyInclude(query, include.Trim());
        }
        return query;
    }
    
    private static IQueryable<T> ApplyInclude<T>(IQueryable<T> query, string include) where T : class
    {
        var properties = include.Split('.');

        if (properties.Length == 1)
        {
            return query.Include(properties[0]);
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression property = Expression.Property(parameter, properties[0]);
        var lambda = Expression.Lambda(property, parameter);

        var method = typeof(EntityFrameworkQueryableExtensions)
            .GetMethods()
            .First(m => m.Name == "Include" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });

        return ApplyThenInclude(query, properties, 1, property.Type);
    }

    private static IQueryable<T> ApplyThenInclude<T>(IQueryable<T> query, string[] properties, int index, Type entityType)
    {
        if (index >= properties.Length)
        {
            return query;
        }

        var parameter = Expression.Parameter(entityType, "x");
        Expression property = Expression.Property(parameter, properties[index]);
        var lambda = Expression.Lambda(property, parameter);

        var method = typeof(EntityFrameworkQueryableExtensions)
            .GetMethods()
            .First(m => m.Name == "ThenInclude" && m.GetParameters().Length == 2)
            .MakeGenericMethod(query.ElementType, entityType, property.Type);

        query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });

        return ApplyThenInclude(query, properties, index + 1, property.Type);
    }
}