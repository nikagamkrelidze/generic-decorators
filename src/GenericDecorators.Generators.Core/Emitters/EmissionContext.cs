using System.Collections.Generic;
using System.Linq;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;
using GenericDecorators.Generators.Core.Parsers.Contexts.SimpleDecorators;
using GenericDecorators.Generators.Core.Symbols;

namespace GenericDecorators.Generators.Core.Emitters;

internal readonly record struct EmissionContext(
    IReadOnlyCollection<SimpleDecoratorContext> SimpleDecoratorContexts,
    TypeContext SimpleInterceptor,
    TypeContext SimpleDecoratorsInstantiator,
    TypeContext HashSetOfStrings,
    TypeContext NotImplementedException)
{
    public EmissionContext(
        IEnumerable<SimpleDecoratorContext> simpleDecoratorContexts,
        SymbolHolder symbolHolder)
            : this(simpleDecoratorContexts.ToList(),
                new TypeContext(symbolHolder.SimpleInterceptor),
                new TypeContext(symbolHolder.SimpleDecoratorInstantiator),
                new TypeContext(symbolHolder.HashSetOfStrings),
                new TypeContext(symbolHolder.NotImplementedException))
    {
    }
}
