using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

internal readonly struct TypeContext
{
    public readonly string FullyQualifiedName { get; }
    public readonly string ShortName { get; }
    public readonly bool IsTypeParameter { get; }

    public TypeContext(ITypeSymbol typeSymbol)
    {
        IsTypeParameter = typeSymbol is ITypeParameterSymbol;

        ShortName = typeSymbol.Name;

        FullyQualifiedName = typeSymbol.ToDisplayString(_fullyQualifiedFormat);

        var typeArguments = new List<TypeContext>();

        if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
        {
            typeArguments.AddRange(namedTypeSymbol.TypeArguments.Select(x => new TypeContext(x)));
        }

        if (typeArguments.Count != 0)
        {
            ShortName += $"<{string.Join(", ", typeArguments.Select(x => x.ShortName))}>";
            FullyQualifiedName += $"<{string.Join(", ", typeArguments.Select(x => x.FullyQualifiedName))}>";
        }
    }

    private static SymbolDisplayFormat _fullyQualifiedFormat = new(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
}
