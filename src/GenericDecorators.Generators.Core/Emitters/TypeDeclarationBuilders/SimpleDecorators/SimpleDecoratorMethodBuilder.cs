using System;
using System.Collections.Generic;
using System.Linq;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecorators;

internal sealed class SimpleDecoratorMethodBuilder
{
    private const string MethodContextVariableName = "methodContext";
    private const string ContainsMethodName = "Contains";

    public static string Build(SimpleDecoratorMethodBuilderContext context)
    {
        var postInterceptorAssignment = string.Join(
            Environment.NewLine,
            context
                .MethodParameters
                .Where(x => x.RefKind == ParameterRefKind.Out || x.RefKind == ParameterRefKind.Ref)
                .Select(x => $"{x.Name} = {MethodContextVariableName}.{context.ParameterNamesInContext[x.Name]};"));

        var interceptorInvocationResultHolder = context.MethodIsVoid ? string.Empty : "var interceptorInvocationResult = ";

        var methodReturn = context.MethodIsVoid ? string.Empty : "return interceptorInvocationResult;";

        return $$"""
                 
                     {{GetMethodDeclaration(context)}}
                     {
                         {{GetIsApplicableCondition(context)}}
                 
                         {{GetMethodContextInitialization(context)}}
                 
                         {{interceptorInvocationResultHolder}} {{GetInterceptorInvocation(context)}};
                 
                         {{postInterceptorAssignment}}
                 
                         {{methodReturn}}
                     }

                 """;
    }

    private static string GetInterceptorInvocation(SimpleDecoratorMethodBuilderContext context)
    {
        var methodContextArgument = context.MethodIsAwaitable ? MethodContextVariableName : $"in {MethodContextVariableName}";

        var tempVariableNames = context.MethodParameters
            .Where(x => x.RefKind != ParameterRefKind.None)
            .ToDictionary(x => x.Name, x => $"temp_{x.Name}_" + Guid.NewGuid().ToString("N").Substring(0, 5));

        var lambdaArguments = string.Join(
            ", ",
            context
                .MethodParameters
                .Select(x =>
                {
                    if (x.RefKind == ParameterRefKind.None)
                    {
                        return $"{MethodContextVariableName}.{context.ParameterNamesInContext[x.Name]}";
                    }
                    else
                    {
                        var typeInArgumentPass = x.RefKind == ParameterRefKind.Out ? x.TypeFullyQualified : string.Empty; // only required for out parameters

                        return $"{RefKindToString(x.RefKind)} {typeInArgumentPass} {tempVariableNames[x.Name]}";
                    }
                }));

        var typeArgs = context.MethodTypeParameters.Count > 0 ? $"<{string.Join(", ", context.MethodTypeParameters)}>" : string.Empty;

        var lambdaBodyInvocation = $"{MethodContextVariableName}.{context.UnderlyingImplementationContextStructureFieldName}.{context.MethodName}{typeArgs}({lambdaArguments})";

        var lambdaInvocationResultHolder = context.MethodIsVoid ? string.Empty : "localResult";

        var lambdaBodyReturn = context.MethodIsVoid ? string.Empty : $"return {lambdaBodyInvocation};";

        var lambdaExpression =
            $@"static ({MethodContextVariableName}) =>
            {{
                {string.Join(
                    Environment.NewLine,
                    context
                        .MethodParameters
                        .Where(x => x.RefKind == ParameterRefKind.In || x.RefKind == ParameterRefKind.Ref)
                        .Select(x => $"var {tempVariableNames[x.Name]} = {MethodContextVariableName}.{context.ParameterNamesInContext[x.Name]};"))}

                {(context.MethodIsVoid ? $"{lambdaBodyInvocation};" : $"var {lambdaInvocationResultHolder} = {lambdaBodyInvocation};")}

                {string.Join(
                    Environment.NewLine,
                    context
                        .MethodParameters
                        .Where(x => x.RefKind == ParameterRefKind.Out || x.RefKind == ParameterRefKind.Ref)
                        .Select(x => $"{MethodContextVariableName}.{context.ParameterNamesInContext[x.Name]} = {tempVariableNames[x.Name]};"))}

                {(context.MethodIsVoid ? string.Empty : $"return {lambdaInvocationResultHolder};")}      
            }}";

        return $"{context.InterceptorFieldName}.{context.InterceptorMethodName}({methodContextArgument}, {lambdaExpression})";
    }

    private static string GetMethodContextInitialization(SimpleDecoratorMethodBuilderContext context)
    {
        var hasGenericTypeParameters = context.MethodTypeParameters.Count > 0;
        string typeArgs = string.Empty;

        if (hasGenericTypeParameters)
        {
            var contextTypeArgs =
                context
                    .MethodTypeParameters
                    .Where(x => context.MethodParameters.Any(y => y.TypeFullyQualified == x))
                    .Distinct();

            if (contextTypeArgs.Any())
            {
                typeArgs = $"<{string.Join(", ", contextTypeArgs)}>";
            }
        }

        return $@"var {MethodContextVariableName} = new {context.ContextStructName}{typeArgs}
        {{
            {string.Join(
                $",{Environment.NewLine}",
                new[] { $"{context.UnderlyingImplementationContextStructureFieldName} = {context.UnderlyingImplementationFieldName}" }
                    .Concat(context
                        .MethodParameters
                        .Where(x => x.RefKind == ParameterRefKind.None || x.RefKind == ParameterRefKind.Ref || x.RefKind == ParameterRefKind.In)
                        .Select(x => $"{context.ParameterNamesInContext[x.Name]} = {x.Name}")))}
        }};";
    }

    private static string GetIsApplicableCondition(SimpleDecoratorMethodBuilderContext context)
    {
        var typeArgs = context.MethodTypeParameters.Count > 0 ? $"<{string.Join(", ", context.MethodTypeParameters)}>" : string.Empty;

        var methodInvocation =
            $"{context.UnderlyingImplementationFieldName}.{context.MethodName}{typeArgs}({string.Join(", ", context.MethodParameters.Select(p => $"{RefKindToString(p.RefKind)} {p.Name}"))})";

        var ifBody = context.MethodIsVoid ? $"{methodInvocation};" : $"return {methodInvocation};";

        return $@"
        if ({context.ApplicableMembersFieldName} != null &&
            !{context.ApplicableMembersFieldName}.{ContainsMethodName}(nameof({context.DefiningInterfaceFullyQualified}.{context.MethodName})))
        {{
            {ifBody}
        }}";
    }

    private static string GetMethodDeclaration(SimpleDecoratorMethodBuilderContext context)
    {
        var returnType = context.MethodIsVoid ? "void" : context.MethodReturnTypeFullyQualified!;
        var parameterList = string.Join(", ", context.MethodParameters.Select(param => $"{RefKindToString(param.RefKind)} {param.TypeFullyQualified} {param.Name}"));
        var genericList = context.MethodTypeParameters.Count > 0 ? $"<{string.Join(", ", context.MethodTypeParameters)}>" : string.Empty;

        return $"{returnType} {context.DefiningInterfaceFullyQualified}.{context.MethodName}{genericList}({parameterList})";
    }

    private static string RefKindToString(ParameterRefKind refKind) =>
        refKind switch
        {
            ParameterRefKind.None => string.Empty,
            ParameterRefKind.Ref => "ref",
            ParameterRefKind.Out => "out",
            ParameterRefKind.In => "in",
            _ => throw new InvalidOperationException($"Unknown ref kind: {refKind}")
        };
}

internal readonly record struct MethodParameterContext(
    ParameterRefKind RefKind,
    string TypeFullyQualified,
    string Name,
    bool IsTypeParameter);

internal readonly record struct SimpleDecoratorMethodBuilderContext(
    string InterfaceFullyQualified,
    string DefiningInterfaceFullyQualified,
    string ContextStructName,
    string InterceptorFieldName,
    string InterceptorMethodName,
    string MethodName,
    bool MethodIsAwaitable,
    bool MethodIsVoid,
    string? MethodReturnTypeFullyQualified,
    IReadOnlyCollection<string> MethodTypeParameters,
    IReadOnlyCollection<MethodParameterContext> MethodParameters,
    string UnderlyingImplementationContextStructureFieldName,
    bool CastUnderlyingImplementationInvocation,
    string UnderlyingImplementationFieldName,
    string ApplicableMembersFieldName,
    Dictionary<string, string> ParameterNamesInContext);
