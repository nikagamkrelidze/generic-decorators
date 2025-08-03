using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Extensions;

internal static class SymbolExtensions
{
    public static string ToFullyQualifiedName(this ISymbol namedTypeSymbol)
        => namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
}
