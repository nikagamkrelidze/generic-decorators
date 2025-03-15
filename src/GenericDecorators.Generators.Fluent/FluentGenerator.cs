#pragma warning disable RS1042
#pragma warning disable RS1036

using GenericDecorators.Generators.Core.Generators;
using GenericDecorators.Generators.GenerationProviders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericDecorators.Generators;

/// <summary>
/// This is the generator that's responsible for actually generating the decorator classes based on the usage
/// of the Builder pattern.
/// </summary>
[Generator]
public class FluentGenerator : ClassicalGenerator<GenericNameSyntax>
{
    public FluentGenerator() : base(new SimpleDecoratorTriggerProvider())
    {
    }
}