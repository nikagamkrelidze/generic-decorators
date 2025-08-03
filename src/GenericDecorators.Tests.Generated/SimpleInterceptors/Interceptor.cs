using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using GenericDecorators.Extensions.Core.BaseInterceptors;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors;

public class Interceptor : SimpleInterceptor
{
    public ConcurrentQueue<string> Invocations { get; private set; }

    public Interceptor(ConcurrentQueue<string> invocations)
    {
        Invocations = invocations;
    }

    public override void Process<TMethodContext>(in TMethodContext methodContext, Action<TMethodContext> internalImplementation)
    {
        Invocations.Enqueue(nameof(Process));

        internalImplementation(methodContext);

        Invocations.Enqueue(nameof(Process));
    }

    public override TOutput ProcessWithReturnType<TMethodContext, TOutput>(in TMethodContext methodContext, Func<TMethodContext, TOutput> internalImplementation)
    {
        Invocations.Enqueue(nameof(ProcessWithReturnType));

        var result = internalImplementation(methodContext);

        Invocations.Enqueue(nameof(ProcessWithReturnType));

        return result;
    }

    public override Task ProcessAsync<TMethodContext>(TMethodContext methodContext, Func<TMethodContext, Task> internalImplementation)
    {
        Invocations.Enqueue(nameof(ProcessAsync));

        var result = base.ProcessAsync(methodContext, internalImplementation);

        Invocations.Enqueue(nameof(ProcessAsync));

        return result;
    }

    public override Task<TOutput> ProcessWithReturnTypeAsync<TMethodContext, TOutput>(TMethodContext methodContext, Func<TMethodContext, Task<TOutput>> internalImplementation)
    {
        Invocations.Enqueue(nameof(ProcessWithReturnTypeAsync));

        var result = base.ProcessWithReturnTypeAsync(methodContext, internalImplementation);

        Invocations.Enqueue(nameof(ProcessWithReturnTypeAsync));

        return result;
    }
#if NETCOREAPP2_0_OR_GREATER

    public override ValueTask ProcessValueTaskAsync<TMethodContext>(TMethodContext methodContext, Func<TMethodContext, ValueTask> internalImplementation)
    {
        Invocations.Enqueue(nameof(ProcessValueTaskAsync));

        var result = base.ProcessValueTaskAsync(methodContext, internalImplementation);

        Invocations.Enqueue(nameof(ProcessValueTaskAsync));

        return result;
    }

    public override ValueTask<TOutput> ProcessWithReturnTypeValueTaskAsync<TMethodContext, TOutput>(TMethodContext methodContext, Func<TMethodContext, ValueTask<TOutput>> internalImplementation)
    {
        Invocations.Enqueue(nameof(ProcessWithReturnTypeValueTaskAsync));

        var result = base.ProcessWithReturnTypeValueTaskAsync(methodContext, internalImplementation);

        Invocations.Enqueue(nameof(ProcessWithReturnTypeValueTaskAsync));

        return result;
    }
#endif
}
