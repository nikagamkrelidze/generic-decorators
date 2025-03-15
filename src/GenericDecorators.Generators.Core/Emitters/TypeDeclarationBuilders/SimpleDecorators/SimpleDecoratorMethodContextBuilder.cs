using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecorators;

internal sealed class SimpleDecoratorMethodContextBuilder
{
    private const string UnderlyingImplementationConstructorParameterName = "underlyingImplementation";

    public static string Build(SimpleDecoratorMethodContextBuilderContext context)
    {
        var fields = new List<string>
        {
            $"public {context.InterfaceFullyQualified} {context.UnderlyingImplementationContextStructureFieldName};"
        };

        fields.AddRange(context.MethodParameters.Select(param =>
            $"public {param.TypeFullyQualified} {context.ParameterNamesInContext[param.Name]};"));

        var constructorParameters = new List<string>
        {
            $"{context.InterfaceFullyQualified} {UnderlyingImplementationConstructorParameterName}"
        };

        constructorParameters.AddRange(context.MethodParameters.Select(param =>
            $"{param.TypeFullyQualified} {param.Name}"));

        var assignments = new List<string>
        {
            $"this.{context.UnderlyingImplementationContextStructureFieldName} = {UnderlyingImplementationConstructorParameterName};"
        };

        assignments.AddRange(context.MethodParameters.Select(param =>
            $"this.{context.ParameterNamesInContext[param.Name]} = {param.Name};"));

        var typeParameters = context.MethodParameters
            .Where(x => x.IsTypeParameter)
            .Select(x => x.TypeFullyQualified)
            .Distinct()
            .ToList();

        return $$"""
                         private struct {{context.ContextStructName}}{{(typeParameters.Count > 0 ? $"<{string.Join(", ", typeParameters)}>" : string.Empty)}}
                         {
                             {{string.Join(Environment.NewLine, fields)}}
                         }
                 """;
    }
}

internal readonly record struct SimpleDecoratorMethodContextBuilderContext(
    string ContextStructName,
    string UnderlyingImplementationContextStructureFieldName,
    string InterfaceFullyQualified,
    IReadOnlyCollection<MethodParameterContext> MethodParameters,
    IReadOnlyDictionary<string, string> ParameterNamesInContext);
