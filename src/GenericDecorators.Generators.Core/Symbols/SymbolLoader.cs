using GenericDecorators.Generators.Core.Extensions;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Symbols;

internal static class SymbolLoader
{
    public const string SimpleInterceptorTypeNameFullyQualified = "GenericDecorators.Extensions.Core.BaseInterceptors.SimpleInterceptor";
    public const string SimpleDecoratorInstantiatorInterfaceFullyQualified = "GenericDecorators.Extensions.Core.Factory.ISimpleDecoratorInstantiator";
    public const string HashSetOfStringsTypeNameFullyQualified = "System.Collections.Generic.HashSet<System.String>";
    public const string NotImplementedException = "System.NotImplementedException";

    public static bool TryLoad(Compilation compilation, out SymbolHolder? symbolHolder)
    {
        var simpleInterceptorSymbol = compilation.GetNamedTypeSymbol(SimpleInterceptorTypeNameFullyQualified);
        var simpleDecoratorInstantiatorSymbol = compilation.GetNamedTypeSymbol(SimpleDecoratorInstantiatorInterfaceFullyQualified);
        var hashSetOfStringSymbol = compilation.GetNamedTypeSymbol(HashSetOfStringsTypeNameFullyQualified);
        var notImplementedException = compilation.GetNamedTypeSymbol(NotImplementedException);

        if (simpleInterceptorSymbol == null ||
            hashSetOfStringSymbol == null ||
            simpleDecoratorInstantiatorSymbol == null ||
            notImplementedException == null)
        {
            symbolHolder = default;
            return false;
        }

        symbolHolder = new(simpleInterceptorSymbol, simpleDecoratorInstantiatorSymbol, hashSetOfStringSymbol, notImplementedException);

        return true;
    }
}
