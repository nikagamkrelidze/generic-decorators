using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

namespace GenericDecorators.Generators.Core.Parsers.Contexts;

/// <summary>
/// The abstraction of a decorator context.
/// </summary>
internal interface IDecoratorContext
{
    /// <summary>
    /// Gets the interface that the decorator implements.
    /// </summary>
    TypeContext Interface { get; }

    /// <summary>
    /// Gets the interceptor that the decorator uses.
    /// </summary>
    TypeContext Interceptor { get; }
}
