using GenericDecorators.Extensions.Core.BaseInterceptors;
using GenericDecorators.Extensions.Fluent.Builders;

namespace GenericDecorators.Extensions.Fluent;

/// <summary>
/// Serves as a starting point for creating decorators using Builder pattern.
/// </summary>
public static class Decorator
{
    /// <summary>
    /// Creates a builder object which can be used to gather configuration for the end simple decorator.
    /// </summary>
    /// <typeparam name="TInterface">The interface the user wants to decorate.</typeparam>
    /// <typeparam name="TInterceptor">The interceptor used for decoration.</typeparam>
    /// <returns>The builder object.</returns>
    public static SimpleDecoratorBuilder<TInterface, TInterceptor> For<TInterface, TInterceptor>()
        where TInterface : class
        where TInterceptor : SimpleInterceptor
    {
        return new SimpleDecoratorBuilder<TInterface, TInterceptor>();
    }
}
