using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GenericDecorators.Generators.Core.Diagnostics;
using GenericDecorators.Generators.Core.Extensions;
using GenericDecorators.Generators.Core.Generators.Triggers;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;
using GenericDecorators.Generators.Core.Parsers.Contexts.SimpleDecorators;
using GenericDecorators.Generators.Core.Symbols;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Parsers;

internal sealed class Parser
{
    private readonly Action<Diagnostic> _reportDiagnostic;
    private readonly SymbolHolder _symbolHolder;
    private readonly CancellationToken _cancellationToken;

    public Parser(
        Action<Diagnostic> reportDiagnostic,
        SymbolHolder symbolHolder,
        CancellationToken cancellationToken)
    {
        _reportDiagnostic = reportDiagnostic;
        _symbolHolder = symbolHolder;
        _cancellationToken = cancellationToken;
    }

    public IEnumerable<SimpleDecoratorContext> Parse(IReadOnlyDictionary<SimpleDecoratorTrigger, List<SyntaxNode>> simpleDecoratorTriggers)
    {
        foreach (var trigger in simpleDecoratorTriggers.Keys)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            if (!TryValidate(trigger, _symbolHolder, nodes: simpleDecoratorTriggers[trigger], out var diagnostics))
            {
                diagnostics.ForEach(_reportDiagnostic);

                continue;
            }

            yield return CreateSimpleDecoratorContext(trigger, _symbolHolder);
        }
    }

    // TODO: review error codes and texts
    private static bool TryValidate(
        SimpleDecoratorTrigger trigger,
        SymbolHolder symbolHolder,
        List<SyntaxNode> nodes,
        out List<Diagnostic> diagnostics)
    {
        var isValid = true;

        diagnostics = new List<Diagnostic>();

        if (trigger.Interface.TypeKind != TypeKind.Interface)
        {
            diagnostics.AddRange(CreateDiagnostics(DiagnosticsDescriptors.InvalidInterfaceSymbol, nodes, trigger.Interface.ToFullyQualifiedName()));
            isValid = false;
        }

        if (!trigger.Interceptor.DerivesFrom(symbolHolder.SimpleInterceptor))
        {
            diagnostics.AddRange(
                CreateDiagnostics(DiagnosticsDescriptors.InvalidInterceptorSymbol, nodes, trigger.Interceptor.ToFullyQualifiedName(), symbolHolder.SimpleInterceptor.ToFullyQualifiedName()));
            isValid = false;
        }

        return isValid;
    }

    private static SimpleDecoratorContext CreateSimpleDecoratorContext(SimpleDecoratorTrigger triggerContext, SymbolHolder symbolHolder)
    {
        var propterties = triggerContext
            .Interface
            .GetAllInterfaceMembers()
            .OfType<IPropertySymbol>()
            .Where(x => !x.IsStatic);

        var methods = triggerContext
            .Interface
            .GetAllInterfaceMembers()
            .OfType<IMethodSymbol>()
            .Where(x => propterties.All(y => !SymbolEqualityComparer.Default.Equals(x.AssociatedSymbol, y)));

        return new SimpleDecoratorContext(
            new TypeContext(triggerContext.Interface),
            new TypeContext(triggerContext.Interceptor),
            triggerContext
                .Interceptor
                .InstanceConstructors[0]
                .Parameters
                .Select(x => new ConstructorParameterContext(
                    Type: new TypeContext(x.Type),
                    Name: x.Name))
                .ToList(),
            methods
                .Select(methodSymbol =>
                {
                    return new SimpleDecoratorMethodContext(
                        DefiningInterfaceType: new TypeContext(methodSymbol.ContainingType),
                        BaseMethodName: triggerContext.Interceptor.GetFirstInterceptorOfKind(symbolHolder.SimpleInterceptor, methodSymbol.GetInterceptorKind()).Name,
                        Name: methodSymbol.Name,
                        Kind: methodSymbol.GetInterceptorKind(),
                        ReturnType: methodSymbol.ReturnsVoid ? null : new TypeContext(methodSymbol.ReturnType),
                        TypeParameters: methodSymbol.TypeParameters.Select(x => x.Name).ToList(),
                        Parameters: methodSymbol
                            .Parameters
                            .Select(parameter => new MethodParameterContext(
                                GetRefKind(parameter),
                                new TypeContext(parameter.Type),
                                parameter.Name))
                            .ToList());
                })
                .ToList(),
            propterties
                .Select(propertySymbol =>
                {
                    return new PropertyContext(
                        DefiningInterfaceType: new TypeContext(propertySymbol.ContainingType),
                        new TypeContext(propertySymbol.Type),
                        propertySymbol.Name,
                        propertySymbol.GetPropertyAccessors());
                })
                .ToList());
    }

    private static ParameterRefKind GetRefKind(IParameterSymbol parameter) =>
        parameter.RefKind switch
        {
            RefKind.None => ParameterRefKind.None,
            RefKind.Ref => ParameterRefKind.Ref,
            RefKind.Out => ParameterRefKind.Out,
            RefKind.In => ParameterRefKind.In,
            _ => throw new InvalidOperationException($"Unexpected ref kind: {parameter.RefKind}")
        };

    private static IEnumerable<Diagnostic> CreateDiagnostics(DiagnosticDescriptor desc, IReadOnlyCollection<SyntaxNode> syntaxNodes, params object?[]? messageArgs)
    {
        foreach (var node in syntaxNodes)
        {
            yield return Diagnostic.Create(desc, node.GetLocation(), messageArgs);
        }
    }
}
