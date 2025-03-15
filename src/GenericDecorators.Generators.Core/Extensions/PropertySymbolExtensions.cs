using System.Collections.Generic;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Extensions;

internal static class PropertySymbolExtensions
{
    public static IReadOnlyCollection<PropertyAccessors> GetPropertyAccessors(this IPropertySymbol propertySymbol)
    {
        var result = new List<PropertyAccessors>();

        if (propertySymbol.GetMethod != null)
        {
            result.Add(PropertyAccessors.Get);
        }

        if (propertySymbol.SetMethod is not null)
        {
            if (propertySymbol.SetMethod.IsInitOnly)
            {
                result.Add(PropertyAccessors.Init);
            }
            else
            {
                result.Add(PropertyAccessors.Set);
            }
        }

        return result;
    }
}