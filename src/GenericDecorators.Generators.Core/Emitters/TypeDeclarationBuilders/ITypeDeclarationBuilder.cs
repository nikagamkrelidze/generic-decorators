namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders;

/// <summary>
/// Represents a type that can build a type declaration.
/// </summary>
/// <typeparam name="TGenerationContext">The context used for building.</typeparam>
internal interface ITypeDeclarationBuilder<in TGenerationContext>
{
    /// <summary>
    /// Builds the type declaration.
    /// </summary>
    /// <param name="context">The context used for building.</param>
    /// <returns>The source code that represents the type declaration.</returns>
    string Build(TGenerationContext context);
}
