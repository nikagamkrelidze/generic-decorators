using System.Threading.Tasks;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.TypeParameters;

public interface ITypeParametersService
{
    void Start<T1, T2, T3>(object a, T2 b, T3 c)
        where T2 : class, new()
        where T3 : T2;

    TResult TryStart<T1, T2, T3, TResult>(object a, T2 b, T3 c)
        where T2 : class, new()
        where T3 : T2;

    Task StartAsync<T1, T2, T3>(object a, T2 b, T3 c)
        where T2 : class, new()
        where T3 : T2;

    Task<TResult> TryStartAsync<T1, T2, T3, TResult>(object a, T2 b, T3 c)
        where T2 : class, new()
        where T3 : T2;
#if NETCOREAPP2_0_OR_GREATER

    ValueTask StartValueAsync<T1, T2, T3>(object a, T2 b, T3 c)
        where T2 : class, new()
        where T3 : T2;

    ValueTask<TResult> TryStartValueAsync<T1, T2, T3, TResult>(object a, T2 b, T3 c)
        where T2 : class, new()
        where T3 : T2;
#endif
    object PropertyWithSetter { set; }
    object PropertyWithGetter { get; }
    object PropertyWithGetterAndSetter { get; set; }
#if NET6_0_OR_GREATER
    object PropertyWithInitializer { init; }
    object PropertyWithGetterAndInitializer { get; init; }
#endif
}
