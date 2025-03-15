using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecorators;

internal static class SimpleDecoratorPrivateFieldsBuilder
{
    public static string GetPrivateFieldsDeclarations(List<(string Type, string Name)> context)
        => $"""
                    {string.Join(Environment.NewLine, context.Select(x => $"private readonly {x.Type} {x.Name};"))}
            """;
}
