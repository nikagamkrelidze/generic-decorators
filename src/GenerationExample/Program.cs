using GenericDecorators.Extensions.Core.BaseInterceptors;
using GenericDecorators.Extensions.Fluent;

Decorator.For<ISomeService, LoggingInterceptor>();
Decorator.For<IAnotherService, LoggingInterceptor>();

public interface ISomeService
{
    void Execute(string parameter);
}

public interface IAnotherService
{
    void PerformAction(int value);
}

public class LoggingInterceptor : SimpleInterceptor
{
    public override void Process<TMethodContext>(in TMethodContext methodContext, Action<TMethodContext> internalImplementation)
    {
        Console.WriteLine("Entering method");
        
        internalImplementation(methodContext);
        
        Console.WriteLine("Exiting method");
    }
}