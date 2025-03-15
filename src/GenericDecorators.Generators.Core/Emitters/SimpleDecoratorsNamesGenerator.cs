using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;
using GenericDecorators.Generators.Core.Parsers.Contexts.SimpleDecorators;

namespace GenericDecorators.Generators.Core.Emitters;

internal static class SimpleDecoratorsNamesGenerator
{
    private const int HashLength = 10;

    public static Dictionary<SimpleDecoratorContext, SimpleDecoratorName> Generate(IReadOnlyCollection<SimpleDecoratorContext> contexts)
    {
        var uniqueShortInterceptorNames = GetUniqueShortFormattedTypeNames(contexts.Select(x => x.Interceptor).ToList());
        var uniqueShortInterfaceNames = GetUniqueShortFormattedTypeNames(contexts.Select(x => x.Interface).ToList());

        return contexts
            .ToDictionary(
                x => x,
                x => new SimpleDecoratorName(
                    Namespace: $"Decorators.{uniqueShortInterceptorNames[x.Interceptor]}.{uniqueShortInterfaceNames[x.Interface]}",
                    ClassName: "Decorator",
                    FileName: $"Decorator_{uniqueShortInterceptorNames[x.Interceptor]}_{uniqueShortInterfaceNames[x.Interface]}.g.cs"));
    }

    private static Dictionary<TypeContext, string> GetUniqueShortFormattedTypeNames(IReadOnlyCollection<TypeContext> interceptorTypes)
    {
        var duplicates = new HashSet<string>(
            interceptorTypes
                .GroupBy(x => x.ShortName)
                .Where(x => x.Any())
                .Select(x => x.Key));

        return interceptorTypes
            .Distinct()
            .ToDictionary(
                x => x,
                x => duplicates.Contains(x.ShortName)
                    ? GetSanitizedTypeName(x.ShortName)
                    : GetSanitizedTypeNameWithHash(x.ShortName, x.FullyQualifiedName));
    }

    // TODO: write implementation that accounts for potential length
    private static string GetSanitizedTypeNameWithHash(string shortName, string typeFullyQualifiedName)
    {
        return $"{GetSanitizedTypeName(shortName)}_{ComputeSHA256Hash(typeFullyQualifiedName)}";
    }

    private static string GetSanitizedTypeName(string typeName)
            => typeName
                .Replace("<", "_")
                .Replace(">", "_")
                .Replace(",", "_")
                .Replace(" ", "_")
                .Replace("__", "_")
                .Trim('_');

    private static string ComputeSHA256Hash(string input)
    {
        using var sha256 = SHA256.Create();

        var inputBytes = Encoding.UTF8.GetBytes(input);

        var hashBytes = sha256.ComputeHash(inputBytes);

        var sb = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)
        {
            _ = sb.Append(hashBytes[i].ToString("X2", CultureInfo.InvariantCulture));
        }

        return sb
            .ToString()
            .Substring(0, HashLength)
            .ToUpperInvariant();
    }
}

internal readonly record struct SimpleDecoratorName(string Namespace, string ClassName, string FileName)
{
    public string FullyQualifiedName => $"global::{Namespace}.{ClassName}";
}
