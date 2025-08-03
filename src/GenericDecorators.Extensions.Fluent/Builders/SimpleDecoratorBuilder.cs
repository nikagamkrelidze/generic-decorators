using System;
using System.Collections.Generic;
using GenericDecorators.Extensions.Core.BaseInterceptors;
using GenericDecorators.Extensions.Core.Factory;

namespace GenericDecorators.Extensions.Fluent.Builders;

/// <summary>
/// Builder for simple decorators.
/// </summary>
/// <typeparam name="TInterface">The interface to be decorated.</typeparam>
/// <typeparam name="TInterceptor">The interceptor to be used.</typeparam>
public record SimpleDecoratorBuilder<TInterface, TInterceptor>
        : IEquatable<SimpleDecoratorBuilder<TInterface, TInterceptor>>
    where TInterface : class
    where TInterceptor : SimpleInterceptor
{
    private HashSet<string>? _applicableMembers;
    private TInterface? _underlyingImplementation;
    private TInterceptor? _interceptor;

    /// <summary>
    /// Specifies the members to which the decorator should be applied.
    /// </summary>
    /// <param name="members">The array of member names, preferably in "nameof({interface}.{methodName})" format.</param>
    /// <returns>The modified builder.</returns>
    public SimpleDecoratorBuilder<TInterface, TInterceptor> ApplyToMembers(params string[] members)
    {
        members = members ?? throw new ArgumentNullException(nameof(members));

        _applicableMembers ??= new HashSet<string>();

        foreach (var member in members)
        {
            _ = _applicableMembers.Add(member);
        }

        return this;
    }

    /// <summary>
    /// Specifies that the decorator should be applied to all members of the interface.
    /// </summary>
    /// <returns>The modified builder.</returns>
    public SimpleDecoratorBuilder<TInterface, TInterceptor> ApplyToAllMembers()
    {
        _applicableMembers = null;

        return this;
    }

    /// <summary>
    /// Specifies the underlying implementation of the interface.
    /// </summary>
    /// <param name="underlyingImplementation">The underlying implementation.</param>
    /// <returns>The modified builder.</returns>
    public SimpleDecoratorBuilder<TInterface, TInterceptor> WithImplementation(TInterface underlyingImplementation)
    {
        _underlyingImplementation = underlyingImplementation;

        return this;
    }

    /// <summary>
    /// Specifies the underlying interceptor.
    /// </summary>
    /// <param name="interceptor">The interceptor to be used for incerception.</param>
    /// <returns>The modified builder.</returns>
    public SimpleDecoratorBuilder<TInterface, TInterceptor> WithInterceptor(TInterceptor interceptor)
    {
        _interceptor = interceptor;

        return this;
    }

    /// <summary>
    /// Instantiates the decorator based on the given builder configuration.
    /// </summary>
    /// <returns>The decorator.</returns>
    /// <exception cref="InvalidOperationException">In case the builder configuration is invalid.</exception>
    public TInterface? Instantiate()
    {
        if (_underlyingImplementation == null)
        {
            throw new InvalidOperationException($"Need to provide underlying implementation via {nameof(WithImplementation)}");
        }

        if (_interceptor == null)
        {
            throw new InvalidOperationException($"Need to provide underlying interceptor via {nameof(WithInterceptor)}");
        }

        return DecoratorsFactory
            .Instance
            .InstantiateSimpleDecorator(
                _underlyingImplementation!,
                _applicableMembers,
                _interceptor!);
    }
}
