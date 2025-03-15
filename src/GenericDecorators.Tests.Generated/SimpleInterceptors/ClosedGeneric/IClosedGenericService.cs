using System.Threading.Tasks;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.ClosedGeneric;

public interface IClosedGenericService<T1, T2>
    where T1 : class, new()
    where T2 : T1
{
    void Start(object a, object b, object? c = null);
    object TryStart(object a, object b, object? c = null);
    Task StartAsync(object a, object b, object? c = null);
    Task<object> TryStartAsync(object a, object b, object? c = null);
#if NETCOREAPP2_0_OR_GREATER
    ValueTask StartValueAsync(object a, object b, object? c = null);
    ValueTask<object> TryStartValueAsync(object a, object b, object? c = null);
#endif
    object PropertyWithSetter { set; }
    object PropertyWithGetter { get; }
    object PropertyWithGetterAndSetter { get; set; }
#if NET6_0_OR_GREATER
    object PropertyWithInitializer { init; }
    object PropertyWithGetterAndInitializer { get; init; }
#endif
}
