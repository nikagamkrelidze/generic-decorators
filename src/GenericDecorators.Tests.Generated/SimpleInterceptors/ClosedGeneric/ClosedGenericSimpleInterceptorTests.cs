using GenericDecorators.Extensions.Fluent;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.ClosedGeneric;

public class ClosedGenericSimpleInterceptorTests : ClosedGenericSimpleInterceptorTestsBase
{
    protected override IClosedGenericService<Type1, Type2>? Instantiate(IClosedGenericService<Type1, Type2> internalImplementation, Interceptor interceptor)
    {
        return Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
            .WithImplementation(internalImplementation)
            .WithInterceptor(interceptor)
            .Instantiate();
    }
}
