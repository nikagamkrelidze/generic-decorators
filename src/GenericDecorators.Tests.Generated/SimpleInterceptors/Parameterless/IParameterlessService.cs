using System.Threading.Tasks;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.Parameterless;

public interface IParameterlessService
{
    void Start();
    object TryStart();
    Task StartAsync();
    Task<object> TryStartAsync();
#if NETCOREAPP2_0_OR_GREATER
    ValueTask StartValueAsync();
    ValueTask<object> TryStartValueAsync();
#endif
    object PropertyWithSetter { set; }
    object PropertyWithGetter { get; }
    object PropertyWithGetterAndSetter { get; set; }
#if NET6_0_OR_GREATER
    object PropertyWithInitializer { init; }
    object PropertyWithGetterAndInitializer { get; init; }
#endif
}
