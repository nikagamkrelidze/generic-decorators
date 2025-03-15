namespace GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

internal readonly record struct MethodParameterContext(
    ParameterRefKind RefKind,
    TypeContext Type,
    string Name);

internal enum ParameterRefKind
{
    None,
    Ref,
    Out,
    In
}