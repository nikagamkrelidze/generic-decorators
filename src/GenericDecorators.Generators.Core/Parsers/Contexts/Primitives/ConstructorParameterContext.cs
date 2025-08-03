namespace GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

internal readonly record struct ConstructorParameterContext(
    TypeContext Type,
    string Name); // todo: can't be generic
