using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders;
using GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecoratorInstantiators;
using GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecorators;

namespace GenericDecorators.Generators.Core.Emitters;

internal sealed class Emitter
{
    private const string SimpleDecoratorsInstantiatorFileName = "SimpleDecoratorsInstantiator.g.cs";

    private readonly ITypeDeclarationBuilder<SimpleDecoratorBuilderContext> _simpleDecoratorSyntaxBuilder;
    private readonly ITypeDeclarationBuilder<SimpleDecoratorInstantiatorBuilderContext> _simpleDecoratorInstantiatorSyntaxBuilder;

    public Emitter(
        ITypeDeclarationBuilder<SimpleDecoratorBuilderContext> simpleDecoratorSyntaxBuilder,
        ITypeDeclarationBuilder<SimpleDecoratorInstantiatorBuilderContext> simpleDecoratorInstantiatorSyntaxBuilder)
    {
        _simpleDecoratorSyntaxBuilder = simpleDecoratorSyntaxBuilder;
        _simpleDecoratorInstantiatorSyntaxBuilder = simpleDecoratorInstantiatorSyntaxBuilder;
    }

    public Emitter()
        : this(
            new SimpleDecoratorBuilder(),
            new SimpleDecoratorInstantiatorBuilder())
    {
    }

    public IEnumerable<SourceFile> Emit(
        EmissionContext context,
        CancellationToken cancellationToken)
    {
        var simpleDecoratorsNames = SimpleDecoratorsNamesGenerator.Generate(context.SimpleDecoratorContexts);

        foreach (var simpleDecoratorContext in context.SimpleDecoratorContexts)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var simpleDecoratorDeclaration = _simpleDecoratorSyntaxBuilder.Build(
                new SimpleDecoratorBuilderContext(
                    simpleDecoratorsNames[simpleDecoratorContext].Namespace,
                    simpleDecoratorsNames[simpleDecoratorContext].ClassName,
                    simpleDecoratorContext,
                    context.HashSetOfStrings.FullyQualifiedName,
                    context.NotImplementedException.FullyQualifiedName));

            yield return new SourceFile(simpleDecoratorsNames[simpleDecoratorContext].FileName, simpleDecoratorDeclaration);
        }

        yield return new SourceFile(
            SimpleDecoratorsInstantiatorFileName,
            _simpleDecoratorInstantiatorSyntaxBuilder.Build(
                new SimpleDecoratorInstantiatorBuilderContext(
                    context.SimpleDecoratorsInstantiator.FullyQualifiedName,
                    context.HashSetOfStrings.FullyQualifiedName,
                    context
                        .SimpleDecoratorContexts
                        .Select(simpleDecoratorContext => new InstantiatorDecoratorSelectorDeclarationSyntaxBuilderContext(
                            simpleDecoratorsNames[simpleDecoratorContext].FullyQualifiedName,
                            simpleDecoratorContext.Interface.FullyQualifiedName,
                            simpleDecoratorContext.Interceptor.FullyQualifiedName,
                            simpleDecoratorContext.InterceptorConstructorParameters.Select(x => x.Type.FullyQualifiedName).ToList())).ToArray())));
    }
}
