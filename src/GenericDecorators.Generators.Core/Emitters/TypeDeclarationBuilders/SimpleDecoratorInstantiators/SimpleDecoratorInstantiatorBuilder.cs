using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecoratorInstantiators;

internal sealed class SimpleDecoratorInstantiatorBuilder : ITypeDeclarationBuilder<SimpleDecoratorInstantiatorBuilderContext>
{
    private const string Namespace = "Decorators.Instantiators"; // TODO: make unique for each type of decorator
    private const string Class = "SimpleDecoratorInstantiator";

    private const string GenericInterfaceType = "TInterface";
    private const string GenericInterceptorType = "TInterceptor";

    private const string GenericInterfaceMethodParameter = "underlyingImplementation";
    private const string ApplicableMembersMethodParameter = "applicableMembers";
    private const string InterceptorMethodParameter = "interceptor";

    private const string InstantiatorMethodName = "Instantiate";

    public string Build(SimpleDecoratorInstantiatorBuilderContext context)
    {
        return $$"""
                 namespace {{Namespace}}
                 {
                     public class {{Class}} : {{context.InstantiatorInterfaceFullyQualifiedName}}
                     {
                         {{GenericInterfaceType}} {{context.InstantiatorInterfaceFullyQualifiedName}}.{{InstantiatorMethodName}}<{{GenericInterfaceType}}, {{GenericInterceptorType}}>(
                             {{GenericInterfaceType}} {{GenericInterfaceMethodParameter}},
                             {{context.HashSetOfStringsFullyQualifiedName}} {{ApplicableMembersMethodParameter}},
                             {{GenericInterceptorType}} {{InterceptorMethodParameter}})
                                 where {{GenericInterfaceType}} : class
                         {
                             {{string.Join(
                                 Environment.NewLine,
                                 context.SimpleDecorators.Select(
                                     simpleDecorator =>
                                         $$"""
                                           
                                                       if (typeof({{GenericInterfaceType}}) == typeof({{simpleDecorator.InterfaceFullyQualifiedName}}) &&
                                                           typeof({{GenericInterceptorType}}) == typeof({{simpleDecorator.InterceptorFullyQualifiedName}}))
                                                       {
                                                           return new {{simpleDecorator.DecoratorFullyQualifiedName}}(
                                                               {{GenericInterfaceMethodParameter}} as {{simpleDecorator.InterfaceFullyQualifiedName}},
                                                               {{ApplicableMembersMethodParameter}},
                                                               {{InterceptorMethodParameter}} as {{simpleDecorator.InterceptorFullyQualifiedName}})
                                                                   as {{GenericInterfaceType}};
                                                       }
                                           """))}}
                 
                             return default;
                         }
                 
                         System.Type {{context.InstantiatorInterfaceFullyQualifiedName}}.GetDecoratorType<{{GenericInterfaceType}}, {{GenericInterceptorType}}>()
                         {
                             {{string.Join(
                                 Environment.NewLine,
                                 context.SimpleDecorators.Select(
                                     simpleDecorator =>
                                         $$"""
                                           
                                                       if (typeof({{GenericInterfaceType}}) == typeof({{simpleDecorator.InterfaceFullyQualifiedName}}) &&
                                                           typeof({{GenericInterceptorType}}) == typeof({{simpleDecorator.InterceptorFullyQualifiedName}}))
                                                       {
                                                           return typeof({{simpleDecorator.DecoratorFullyQualifiedName}});
                                                       }
                                           """))}}
                 
                             return default;
                         }
                     }
                 }

                 """;
    }
}

internal readonly record struct SimpleDecoratorInstantiatorBuilderContext(
    string InstantiatorInterfaceFullyQualifiedName,
    string HashSetOfStringsFullyQualifiedName,
    IReadOnlyCollection<InstantiatorDecoratorSelectorDeclarationSyntaxBuilderContext> SimpleDecorators);

internal readonly record struct InstantiatorDecoratorSelectorDeclarationSyntaxBuilderContext(
    string DecoratorFullyQualifiedName,
    string InterfaceFullyQualifiedName,
    string InterceptorFullyQualifiedName,
    IReadOnlyCollection<string> InterceptorConstructorParameterFullyQualifiedNames);
