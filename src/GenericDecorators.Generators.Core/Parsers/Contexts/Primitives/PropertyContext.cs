using System.Collections.Generic;

namespace GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

internal readonly record struct PropertyContext(
    TypeContext DefiningInterfaceType,
    TypeContext Type,
    string Name,
    IReadOnlyCollection<PropertyAccessors> AccessModifiers);

internal enum PropertyAccessors
{
    Set,
    Get,
    Init
}
