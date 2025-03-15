using System.Threading.Tasks;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.RefParameterized;

public interface IRefParameterizedService
{
    void Start(in object a, ref object b, out object c, object d);
    object TryStart(in object a, ref object b, out object c, object d);
    Task StartAsync(in object a, ref object b, out object c, object d);
    Task<object> TryStartAsync(in object a, ref object b, out object c, object d);
#if NETCOREAPP2_0_OR_GREATER
    ValueTask StartValueAsync(in object a, ref object b, out object c, object d);
    ValueTask<object> TryStartValueAsync(in object a, ref object b, out object c, object d);
#endif
    object PropertyWithSetter { set; }
    object PropertyWithGetter { get; }
    object PropertyWithGetterAndSetter { get; set; }
#if NET6_0_OR_GREATER
    object PropertyWithInitializer { init; }
    object PropertyWithGetterAndInitializer { get; init; }
#endif
}
