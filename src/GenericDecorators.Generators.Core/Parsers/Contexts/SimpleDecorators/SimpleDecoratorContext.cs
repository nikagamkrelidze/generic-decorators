using System.Collections.Generic;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

namespace GenericDecorators.Generators.Core.Parsers.Contexts.SimpleDecorators;

internal readonly record struct SimpleDecoratorContext(
    TypeContext Interface,
    TypeContext Interceptor,
    IReadOnlyCollection<ConstructorParameterContext> InterceptorConstructorParameters,
    IReadOnlyCollection<SimpleDecoratorMethodContext> Methods,
    IReadOnlyCollection<PropertyContext> Properties)
    : IDecoratorContext;
