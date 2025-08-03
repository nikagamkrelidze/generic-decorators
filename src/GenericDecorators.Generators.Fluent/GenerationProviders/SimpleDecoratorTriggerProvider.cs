using System.Linq;
using GenericDecorators.Generators.Core.Extensions;
using GenericDecorators.Generators.Core.Generators.Triggers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericDecorators.Generators.GenerationProviders;

internal sealed class SimpleDecoratorTriggerProvider : ISimpleDecoratorTriggerProvider<GenericNameSyntax>
{
    public const string StaticDecoratorClassFullyQualified = "GenericDecorators.Extensions.Fluent.Decorator";
    public const string SimpleDecoratorGenerationTrigger = "For";

    public bool ShouldTriggerDecoratorGeneration(GenericNameSyntax genericNameSyntax)
    {
        return genericNameSyntax.Identifier.ToString().Contains(SimpleDecoratorGenerationTrigger);
    }

    public bool TryParseContext(
        Compilation compilation,
        GenericNameSyntax genericNameSyntax,
        out SimpleDecoratorTrigger? trigger)
    {
        trigger = null;

        var staticDecoratorSymbol = compilation
            .GetTypeByMetadataName(StaticDecoratorClassFullyQualified);

        var simpleDecoratorGenerationTriggerMethodSymbol = staticDecoratorSymbol?
            .GetMembers()
            .OfType<IMethodSymbol>()
            .FirstOrDefault();

        if (simpleDecoratorGenerationTriggerMethodSymbol == null)
        {
            return false;
        }

        if (!genericNameSyntax.GetMethodSymbol(compilation)!.OriginalDefinition!.Equals(simpleDecoratorGenerationTriggerMethodSymbol, SymbolEqualityComparer.Default))
        {
            return false;
        }

        var interfaceSymbol = genericNameSyntax.TypeArgumentList.Arguments[0].GetNamedTypeSymbol(compilation);

        var interceptorSymbol = genericNameSyntax.TypeArgumentList.Arguments[1].GetNamedTypeSymbol(compilation);

        if (interceptorSymbol == null || interfaceSymbol == null)
        {
            return false;
        }

        trigger = new SimpleDecoratorTrigger(interfaceSymbol, interceptorSymbol);

        return true;
    }
}
