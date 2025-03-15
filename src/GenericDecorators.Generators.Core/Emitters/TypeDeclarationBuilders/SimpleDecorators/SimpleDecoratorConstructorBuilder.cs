using System.Collections.Generic;
using System.Linq;

namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecorators;

internal static class SimpleDecoratorConstructorBuilder
{
    public static string GetConstructorDeclaration(
        string className,
        IReadOnlyCollection<(string TypeFullyQualified, string PrivateFieldName, string ConstructorParamName)> privateFieldAssignments)
        => $$"""
             
                     public {{className}}({{string.Join(", ", privateFieldAssignments.Select(t => $"{t.TypeFullyQualified} {t.ConstructorParamName}"))}})
                     {
                     {{string.Join("\n    ", privateFieldAssignments.Select(t => $"this.{t.PrivateFieldName} = {t.ConstructorParamName};"))}}
                     }
             """;
}
