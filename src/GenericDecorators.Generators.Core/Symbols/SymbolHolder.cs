using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Symbols;

internal sealed record class SymbolHolder(
    INamedTypeSymbol SimpleInterceptor,
    INamedTypeSymbol SimpleDecoratorInstantiator,
    ITypeSymbol HashSetOfStrings,
    INamedTypeSymbol NotImplementedException);