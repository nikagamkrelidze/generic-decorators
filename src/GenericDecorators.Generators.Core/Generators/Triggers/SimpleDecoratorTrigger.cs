using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Generators.Triggers;

/// <summary>
/// The trigger for the simple generator.
/// </summary>
/// <param name="Interface">The interface that should be decorated.</param>
/// <param name="Interceptor">The interceptor that should be used to decorate the interface.</param>
public readonly record struct SimpleDecoratorTrigger(INamedTypeSymbol Interface, INamedTypeSymbol Interceptor);