using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericDecorators.Generators.Core.Extensions;

/// <summary>
/// Contains extension methods for <see cref="TypeSyntax"/>.
/// </summary>
public static class TypeSyntaxExtensions
{
    /// <summary>
    /// Returns the <see cref="INamedTypeSymbol"/> for the specified <see cref="TypeSyntax"/> in the context of the specified <see cref="Compilation"/>.
    /// </summary>
    /// <param name="typeSyntax">The <see cref="TypeSyntax"/> to get the <see cref="INamedTypeSymbol"/> for.</param>
    /// <param name="compilation">The <see cref="Compilation"/> to get the <see cref="INamedTypeSymbol"/> in the context of.</param>
    /// <returns>The <see cref="INamedTypeSymbol"/> for the specified <see cref="TypeSyntax"/> in the context of the specified <see cref="Compilation"/>.</returns>
    public static INamedTypeSymbol? GetNamedTypeSymbol(this TypeSyntax typeSyntax, Compilation compilation)
    {
        var semanticModel = compilation.GetSemanticModel(typeSyntax.SyntaxTree);

        return semanticModel.GetTypeInfo(typeSyntax).Type as INamedTypeSymbol;
    }
}
