using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Extensions;

/// <summary>
/// Compilation extensions.
/// </summary>
public static class CompilationExtensions
{
    private static readonly char[] _angularBrackets = new[] { '<', '>' };
    private static readonly char[] _comma = new[] { ',', ' ' };

    /// <summary>
    /// Gets the named type symbol for the specified type from the compilation.
    /// </summary>
    /// <param name="compilation">The compilation.</param>
    /// <param name="typeName">The type name to get the symbol for.</param>
    /// <returns>The named type symbol.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the type cannot be found.</exception>
    public static INamedTypeSymbol GetNamedTypeSymbol(this Compilation compilation, string typeName)
    {
        // Split the type name into parts based on '<' and '>', handling nested generic types
        var parts = typeName.Split(_angularBrackets, StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim(_comma))
                            .ToArray();

        // Start with the non-generic type name
        var baseTypeName = parts[0];
        var genericArity = parts.Length - 1;
        if (genericArity > 0)
        {
            baseTypeName += "`" + genericArity;
        }

        var baseType = compilation.GetTypeByMetadataName(baseTypeName);
        if (baseType == null)
        {
            throw new InvalidOperationException($"Could not find type {typeName}");
        }

        // If the type is not generic, return it
        if (genericArity == 0)
        {
            return baseType;
        }

        // Recursively get the type symbols for the generic type arguments
        var typeArguments = parts.Skip(1).Select(arg => GetNamedTypeSymbol(compilation, arg)).ToArray();

        // Construct the generic type with the type arguments
        return baseType.Construct(typeArguments);
    }

    private static INamedTypeSymbol GetNamedTypeSymbol(this Compilation compilation, Type type)
    {
        if (!type.IsConstructedGenericType)
        {
            return compilation.GetTypeByMetadataName(type.FullName!) ?? throw new InvalidOperationException($"Could not find type {type.FullName}");
        }

        var typeArgumentsTypeInfos = type.GenericTypeArguments.Select(compilation.GetNamedTypeSymbol);

        var openType = type.GetGenericTypeDefinition();

        var typeSymbol = compilation.GetTypeByMetadataName(openType.FullName!) ?? throw new InvalidOperationException($"Could not find type {openType.FullName}");

        return typeSymbol.Construct(typeArgumentsTypeInfos.ToArray<ITypeSymbol>());
    }
}
