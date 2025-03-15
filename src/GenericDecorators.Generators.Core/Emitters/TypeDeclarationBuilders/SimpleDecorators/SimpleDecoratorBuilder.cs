using System;
using System.Collections.Generic;
using System.Linq;
using GenericDecorators.Generators.Core.Extensions;
using GenericDecorators.Generators.Core.Parsers.Contexts.SimpleDecorators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecorators;

internal sealed class SimpleDecoratorBuilder : ITypeDeclarationBuilder<SimpleDecoratorBuilderContext>
{
    private const string UnderlyingImplementationContextStructureFieldName = "underlyingImplementation";

    private const string UnderlyingImplementationFieldName = "_underlyingImplementation";
    private const string ApplicableMembersFieldName = "_applicableMembers";
    private const string InterceptorFieldName = "_interceptor";

    private const string UnderlyingImplementationConstructorParamName = "underlyingImplementation";
    private const string ApplicableMembersConstructorParamName = "applicableMembers";
    private const string InterceptorConstructorParamName = "interceptor";

    public string Build(SimpleDecoratorBuilderContext context)
    {
        var privateFieldDeclarations = SimpleDecoratorPrivateFieldsBuilder.GetPrivateFieldsDeclarations(
            new List<(string, string)>
            {
                (context.SimpleDecoratorContext.Interface.FullyQualifiedName, UnderlyingImplementationFieldName),
                (context.HashSetOfStringsFullyQualifiedName, ApplicableMembersFieldName),
                (context.SimpleDecoratorContext.Interceptor.FullyQualifiedName, InterceptorFieldName)
            });

        var constructorDeclaration = SimpleDecoratorConstructorBuilder.GetConstructorDeclaration(
            context.Class,
            new List<(string, string, string)>
            {
                new(context.SimpleDecoratorContext.Interface.FullyQualifiedName,
                    UnderlyingImplementationFieldName,
                    UnderlyingImplementationConstructorParamName),
                new(context.HashSetOfStringsFullyQualifiedName,
                    ApplicableMembersFieldName,
                    ApplicableMembersConstructorParamName),
                new(context.SimpleDecoratorContext.Interceptor.FullyQualifiedName,
                    InterceptorFieldName,
                    InterceptorConstructorParamName)
            });

        var propertyDelcarations = context
            .SimpleDecoratorContext
            .Properties
            .Select(propertyContext => SimpleDecoratorPropertiesBuilder.GeneratePropertyFromContext(
                    propertyContext,
                    context.SimpleDecoratorContext.Interface.FullyQualifiedName,
                    UnderlyingImplementationFieldName,
                    context.NotImplementedExceptionFullyQualifiedName));

        var methods = GetMethodContext(context.SimpleDecoratorContext)
                .Select(x => @$"
        {SimpleDecoratorMethodContextBuilder.Build(x.Structure)}
        {SimpleDecoratorMethodBuilder.Build(x.Method)}");

        var code = $$"""
                     namespace {{context.Namespace}}
                     {
                         public class {{context.Class}} : {{context.SimpleDecoratorContext.Interface.FullyQualifiedName}}
                         {
                             {{privateFieldDeclarations}}
                     
                             {{constructorDeclaration}}
                     
                             {{string.Join(Environment.NewLine, propertyDelcarations)}}
                     
                             {{string.Join(Environment.NewLine, methods)}}
                         }
                     }
                     """;

        return SyntaxFactory
            .ParseSyntaxTree(code)
            .GetRoot()
            .NormalizeWhitespace()
            .ToFullString();
    }

    private static IEnumerable<(SimpleDecoratorMethodContextBuilderContext Structure, SimpleDecoratorMethodBuilderContext Method)> GetMethodContext(SimpleDecoratorContext context)
    {
        int i = 0;

        foreach (var method in context.Methods)
        {
            var contextStructName = $"{method.Name}MethodContext_{i++}";

            var parameterNamesInContext = method
                .Parameters
                .ToDictionary(
                    x => x.Name,
                    x => x.Name);

            var methodParameters = method
                .Parameters
                .Select(x => new MethodParameterContext(x.RefKind, x.Type.FullyQualifiedName, x.Name, x.Type.IsTypeParameter))
                .ToList();

            var methodDeclarationContext = new SimpleDecoratorMethodBuilderContext(
                InterfaceFullyQualified: context.Interface.FullyQualifiedName,
                DefiningInterfaceFullyQualified: method.DefiningInterfaceType.FullyQualifiedName,
                ContextStructName: contextStructName,
                InterceptorFieldName: InterceptorFieldName,
                InterceptorMethodName: method.BaseMethodName,
                MethodName: method.Name,
                MethodIsAwaitable: method.Kind != InterceptorKind.Void && method.Kind != InterceptorKind.Value,
                MethodIsVoid: method.Kind == InterceptorKind.Void,
                MethodReturnTypeFullyQualified: method.ReturnType.HasValue ? method.ReturnType.Value.FullyQualifiedName : null,
                MethodTypeParameters: method.TypeParameters,
                MethodParameters: methodParameters,
                UnderlyingImplementationContextStructureFieldName: UnderlyingImplementationContextStructureFieldName,
                CastUnderlyingImplementationInvocation:
                    context.Methods.Any(x => x != method && x.Name == method.Name && method.DefiningInterfaceType.FullyQualifiedName != x.DefiningInterfaceType.FullyQualifiedName),
                UnderlyingImplementationFieldName: UnderlyingImplementationFieldName,
                ApplicableMembersFieldName: ApplicableMembersFieldName,
                ParameterNamesInContext: parameterNamesInContext);

            var structDeclarationContext = new SimpleDecoratorMethodContextBuilderContext(
                contextStructName,
                UnderlyingImplementationContextStructureFieldName,
                context.Interface.FullyQualifiedName,
                methodParameters,
                parameterNamesInContext);

            yield return (structDeclarationContext, methodDeclarationContext);
        }
    }
}

internal readonly record struct SimpleDecoratorBuilderContext(
    string Namespace,
    string Class,
    SimpleDecoratorContext SimpleDecoratorContext,
    string HashSetOfStringsFullyQualifiedName,
    string NotImplementedExceptionFullyQualifiedName);
