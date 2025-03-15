using System.Collections.Generic;
using GenericDecorators.Generators.Core.Extensions;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

namespace GenericDecorators.Generators.Core.Parsers.Contexts.SimpleDecorators;

internal readonly record struct SimpleDecoratorMethodContext(
    TypeContext DefiningInterfaceType,
    string BaseMethodName,
    string Name,
    InterceptorKind Kind,
    TypeContext? ReturnType,
    IReadOnlyCollection<string> TypeParameters,
    IReadOnlyCollection<MethodParameterContext> Parameters);
