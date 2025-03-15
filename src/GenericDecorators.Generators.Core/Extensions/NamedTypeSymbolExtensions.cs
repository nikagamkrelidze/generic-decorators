using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Extensions;

/// <summary>
/// Contains extension methods for <see cref="INamedTypeSymbol"/>.
/// </summary>
internal static class NamedTypeSymbolExtensions
{
    /// <summary>
    /// Determines whether the specified <see cref="INamedTypeSymbol"/> is an open generic type.
    /// </summary>
    /// <param name="typeSymbol">The <see cref="INamedTypeSymbol"/> to determine whether it is an open generic type.</param>
    /// <returns>Whether the specified <see cref="INamedTypeSymbol"/> is an open generic type.</returns>
    public static bool IsOpenGenericType(this ITypeSymbol typeSymbol)
    {
        return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
            namedTypeSymbol.IsGenericType &&
            (namedTypeSymbol.TypeArguments.OfType<ITypeParameterSymbol>().Any() || namedTypeSymbol.TypeArguments.Any(x => x.IsOpenGenericType()));
    }

    /// <summary>
    /// Determines whether the specified <see cref="INamedTypeSymbol"/> is derived from the specified <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="namedTypeSymbol">The <see cref="INamedTypeSymbol"/> to determine whether it is derived from the specified <see cref="INamedTypeSymbol"/>.</param>
    /// <param name="another">The <see cref="INamedTypeSymbol"/> to determine whether the specified <see cref="INamedTypeSymbol"/> is derived from.</param>
    /// <returns>Whether the specified <see cref="INamedTypeSymbol"/> is derived from the specified <see cref="INamedTypeSymbol"/>.</returns>
    public static bool DerivesFrom(this INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol another)
    {
        var current = namedTypeSymbol;

        while (current != null && !current.Equals(another, SymbolEqualityComparer.Default))
        {
            current = current.BaseType;
        }

        return current != null;
    }

    /// <summary>
    /// Retrieves the first interceptor of the specified kind in the inheritance hierarchy of the specified <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="namedTypeSymbol">The <see cref="INamedTypeSymbol"/> to retrieve the first interceptor of the specified kind in the inheritance hierarchy.</param>
    /// <param name="simpleInterceptorSymbol">The <see cref="INamedTypeSymbol"/> of base SimpleInterceptor.</param>
    /// <param name="interceptorKind">The <see cref="InterceptorKind"/> to retrieve the first interceptor of.</param>
    /// <returns>The first interceptor of the specified kind in the inheritance hierarchy of the specified <see cref="INamedTypeSymbol"/>.</returns>
    public static IMethodSymbol GetFirstInterceptorOfKind(this INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol simpleInterceptorSymbol, InterceptorKind interceptorKind)
    {
        IMethodSymbol? symbol = null;

        var interceptorMethods = new List<IMethodSymbol>();

        do
        {
            symbol = namedTypeSymbol!
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => x.MethodKind != MethodKind.Constructor)
                .FirstOrDefault(x => x.IsInterceptor(simpleInterceptorSymbol) && x.GetInterceptorKind() == interceptorKind);

            namedTypeSymbol = namedTypeSymbol.BaseType!;
        }
        while (symbol == null);

        return symbol;
    }

    public static IEnumerable<ISymbol> GetAllInterfaceMembers(this INamedTypeSymbol interfaceSymbol)
    {
        if (interfaceSymbol.TypeKind != TypeKind.Interface)
        {
            return Array.Empty<ISymbol>();
        }

        // Get members of the current interface
        var members = new List<ISymbol>(interfaceSymbol.GetMembers());

        // Get members of all base interfaces
        foreach (var baseInterface in interfaceSymbol.AllInterfaces)
        {
            members.AddRange(baseInterface.GetAllInterfaceMembers());
        }

        return members.Distinct(SymbolEqualityComparer.Default);
    }
}