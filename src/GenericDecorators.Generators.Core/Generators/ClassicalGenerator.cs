using System.Collections.Generic;
using System.Linq;
using GenericDecorators.Generators.Core.Emitters;
using GenericDecorators.Generators.Core.Generators.Triggers;
using GenericDecorators.Generators.Core.Parsers;
using GenericDecorators.Generators.Core.Symbols;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Generators;

/// <summary>
/// This type is the entry point for the non-incremental generator.
/// </summary>
/// <typeparam name="TSyntaxNode">The type of node that triggers the generation.</typeparam>
#pragma warning disable RS1042
public class ClassicalGenerator<TSyntaxNode> : ISourceGenerator
#pragma warning restore RS1042
    where TSyntaxNode : SyntaxNode
{
    private readonly ISimpleDecoratorTriggerProvider<TSyntaxNode> _simpleDecoratorTriggerProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClassicalGenerator{TSyntaxNode}"/> class.
    /// </summary>
    /// <param name="simpleDecoratorTriggerProvider">The trigger provider for the generator.</param>
    public ClassicalGenerator(ISimpleDecoratorTriggerProvider<TSyntaxNode> simpleDecoratorTriggerProvider)
    {
        _simpleDecoratorTriggerProvider = simpleDecoratorTriggerProvider;
    }

    /// <summary>
    /// Initializes the generator.
    /// </summary>
    /// <param name="context">The generator initialization context.</param>
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SimpleDecoratorSyntaxReceiver(_simpleDecoratorTriggerProvider));
    }

    /// <summary>
    /// Executes the generator.
    /// </summary>
    /// <param name="context">The generator execution context.</param>
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxReceiver as SimpleDecoratorSyntaxReceiver;

        if (receiver == null || receiver.SimpleDecoratorTriggers.Count == 0)
        {
            // nothing to do yet
            return;
        }

        if (!SymbolLoader.TryLoad(context.Compilation, out var symbolHolder))
        {
            // Not eligible compilation
            return;
        }

        var contexts = receiver
            .SimpleDecoratorTriggers
                .Select(syntaxNode =>
                {
                    _ = _simpleDecoratorTriggerProvider.TryParseContext(context.Compilation, syntaxNode, out var trigger);

                    return (Context: trigger, Node: syntaxNode);
                })
                .Where(x => x.Context != null)
                .GroupBy(x => x.Context!, x => x.Node)
                .ToDictionary(x => x.Key!.Value, x => x.Select(y => y as SyntaxNode).ToList());

        var parser = new Parser(context.ReportDiagnostic, symbolHolder!, context.CancellationToken);

        var simpleDecoratorContexts = parser.Parse(contexts);

        var emitter = new Emitter();

        var generatedFiles = emitter.Emit(new EmissionContext(simpleDecoratorContexts, symbolHolder!), context.CancellationToken);

        foreach (var file in generatedFiles)
        {
            context.AddSource(file.FileName, file.SourceCode);
        }
    }

    private sealed class SimpleDecoratorSyntaxReceiver : ISyntaxReceiver
    {
        private readonly ISimpleDecoratorTriggerProvider<TSyntaxNode> _simpleDecoratorTriggerProvider;

        public SimpleDecoratorSyntaxReceiver(ISimpleDecoratorTriggerProvider<TSyntaxNode> simpleDecoratorTriggerProvider)
        {
            _simpleDecoratorTriggerProvider = simpleDecoratorTriggerProvider;
            SimpleDecoratorTriggers = new();
        }

        public HashSet<TSyntaxNode> SimpleDecoratorTriggers { get; }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TSyntaxNode sn)
            {
                if (_simpleDecoratorTriggerProvider.ShouldTriggerDecoratorGeneration(sn))
                {
                    _ = SimpleDecoratorTriggers.Add(sn);
                }
            }
        }
    }
}