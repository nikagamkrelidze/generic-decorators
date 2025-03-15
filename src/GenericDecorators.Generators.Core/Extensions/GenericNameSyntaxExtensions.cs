using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericDecorators.Generators.Core.Extensions;

public static class GenericNameSyntaxExtensions
{
    public static IMethodSymbol? GetMethodSymbol(this GenericNameSyntax genericNameSyntax, Compilation compilation)
    {
        var semanticModel = compilation.GetSemanticModel(genericNameSyntax.SyntaxTree);

        return semanticModel.GetSymbolInfo(genericNameSyntax).Symbol as IMethodSymbol;
    }
}