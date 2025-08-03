using System;
using System.Threading.Tasks;

namespace GenericDecorators.Extensions.Core.BaseInterceptors;

/// <summary>
/// A base class for simple interceptors that does not need access to individual parameters of methods.
/// </summary>
public abstract class SimpleInterceptor
{
    /// <summary>
    /// Gets invoked for invocation of all void methods.
    /// </summary>
    /// <param name="methodContext">The object that's encapsulating all the underlying method's specifics.</param>
    /// <param name="internalImplementation">The Action that represents the actual invocation of the underlying method.</param>
    /// <typeparam name="TMethodContext">The type generated specifically on per-method basis for encapsulating the underlying method's specifics.</typeparam>
    public virtual void Process<TMethodContext>(
        in TMethodContext methodContext,
        Action<TMethodContext> internalImplementation)
    {
        internalImplementation(methodContext);
    }

    /// <summary>
    /// Gets invoked for invocation of all methods that return non-awaitable types.
    /// </summary>
    /// <param name="methodContext">The object that's encapsulating all the underlying method's specifics.</param>
    /// <param name="internalImplementation">The Func that represents the actual invocation of the underlying method.</param>
    /// <typeparam name="TMethodContext">The type generated specifically on per-method basis for encapsulating the underlying method's specifics.</typeparam>
    /// <typeparam name="TOutput">The return type of the underlying method.</typeparam>
    /// <returns>The result of the invocation of the actual underlying method.</returns>
    public virtual TOutput ProcessWithReturnType<TMethodContext, TOutput>(
        in TMethodContext methodContext,
        Func<TMethodContext, TOutput> internalImplementation)
    {
        return internalImplementation(methodContext);
    }

    /// <summary>
    /// Gets invoked for invocation of all methods that return Task.
    /// </summary>
    /// <param name="methodContext">The object that's encapsulating all the underlying method's specifics.</param>
    /// <param name="internalImplementation">The Func that represents the actual invocation of the underlying method.</param>
    /// <typeparam name="TMethodContext">The type generated specifically on per-method basis for encapsulating the underlying method's specifics.</typeparam>
    /// <returns>The result of the invocation of the actual underlying method.</returns>
    public virtual Task ProcessAsync<TMethodContext>(
        TMethodContext methodContext,
        Func<TMethodContext, Task> internalImplementation)
    {
        return internalImplementation(methodContext);
    }

    /// <summary>
    /// Gets invoked for invocation of all methods that return Task&lt;&gt;.
    /// </summary>
    /// <param name="methodContext">The object that's encapsulating all the underlying method's specifics.</param>
    /// <param name="internalImplementation">The Func that represents the actual invocation of the underlying method.</param>
    /// <typeparam name="TMethodContext">The type generated specifically on per-method basis for encapsulating the underlying method's specifics.</typeparam>
    /// <typeparam name="TOutput">The type which is returned after the Task&lt;&gt; is awaited.</typeparam>
    /// <returns>The result of the invocation of the actual underlying method.</returns>
    public virtual Task<TOutput> ProcessWithReturnTypeAsync<TMethodContext, TOutput>(
        TMethodContext methodContext,
        Func<TMethodContext, Task<TOutput>> internalImplementation)
    {
        return internalImplementation(methodContext);
    }

#if NETCOREAPP2_0_OR_GREATER

    /// <summary>
    /// Gets invoked for invocation of all methods that return ValueTask.
    /// </summary>
    /// <param name="methodContext">The object that's encapsulating all the underlying method's specifics.</param>
    /// <param name="internalImplementation">The Func that represents the actual invocation of the underlying method.</param>
    /// <typeparam name="TMethodContext">The type generated specifically on per-method basis for encapsulating the underlying method's specifics.</typeparam>
    /// <returns>The result of the invocation of the actual underlying method.</returns>
    public virtual ValueTask ProcessValueTaskAsync<TMethodContext>(
        TMethodContext methodContext,
        Func<TMethodContext, ValueTask> internalImplementation)
    {
        return internalImplementation(methodContext);
    }

    /// <summary>
    /// Gets invoked for invocation of all methods that return ValueTask&lt;&gt;.
    /// </summary>
    /// <param name="methodContext">The object that's encapsulating all the underlying method's specifics.</param>
    /// <param name="internalImplementation">The Func that represents the actual invocation of the underlying method.</param>
    /// <typeparam name="TMethodContext">The type generated specifically on per-method basis for encapsulating the underlying method's specifics.</typeparam>
    /// <typeparam name="TOutput">The type which is returned after the ValueTask&lt;&gt; is awaited.</typeparam>
    /// <returns>The result of the invocation of the actual underlying method.</returns>
    public virtual ValueTask<TOutput> ProcessWithReturnTypeValueTaskAsync<TMethodContext, TOutput>(
        TMethodContext methodContext,
        Func<TMethodContext, ValueTask<TOutput>> internalImplementation)
    {
        return internalImplementation(methodContext);
    }

#endif
}