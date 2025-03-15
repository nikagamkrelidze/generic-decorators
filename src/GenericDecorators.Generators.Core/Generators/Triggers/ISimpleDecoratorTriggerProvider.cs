using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Generators.Triggers;

/// <summary>
/// The trigger provider for the generator.
/// </summary>
/// <typeparam name="TSyntaxNode">The type of node that triggers the generation.</typeparam>
public interface ISimpleDecoratorTriggerProvider<TSyntaxNode>
    where TSyntaxNode : SyntaxNode
{
    /// <summary>
    /// Determines if the syntax node might trigger the generation of a decorator.
    /// </summary>
    /// <returns>Whether the syntax node might trigger the generation of a decorator.</returns>
    bool ShouldTriggerDecoratorGeneration(TSyntaxNode syntaxNode);

    /// <summary>
    /// Parses the syntax node to determine if it's a valid trigger for the generation of a decorator.
    /// </summary>
    /// <param name="compilation">The compilation.</param>
    /// <param name="syntaxNode">The syntax node to parse.</param>
    /// <param name="trigger">The trigger if the syntax node is a valid trigger for the generation of a decorator.</param>
    /// <returns>Whether the syntax node is a valid trigger for the generation of a decorator.</returns>
    bool TryParseContext(Compilation compilation, TSyntaxNode syntaxNode, out SimpleDecoratorTrigger? trigger);
}
