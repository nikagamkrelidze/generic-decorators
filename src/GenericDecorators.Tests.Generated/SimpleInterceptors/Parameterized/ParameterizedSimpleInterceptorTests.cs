using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GenericDecorators.Extensions.Fluent;
using Moq;
using NUnit.Framework;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.Parameterized;

public static class ParameterizedSimpleInterceptorTests
{
    [Test]
    public static void Process()
    {
        var invocations = new ConcurrentQueue<string>();

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock.Setup(x => x.Start(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IParameterizedService.Start)));

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        decoratedInstance.Start(a, b, c);

        serviceMock.Verify(x => x.Start(a, b, c), Times.Once);

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.Process),
                        nameof(IParameterizedService.Start),
                        nameof(Interceptor.Process)
                    }));
    }

    [Test]
    public static void ProcessWithReturnType()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedResult = new object();

        var a = new object();
        var b = new object();
        var c = new object();

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .Setup(x => x.TryStart(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IParameterizedService.TryStart)))
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStart(a, b, c);

        serviceMock.Verify(x => x.TryStart(a, b, c), Times.Once);

        Assert.That(result, Is.SameAs(expectedResult));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnType),
                        nameof(IParameterizedService.TryStart),
                        nameof(Interceptor.ProcessWithReturnType)
                    }));
    }

    [Test]
    public static void ProcessAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var task = new Task(() => { });

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .Setup(x => x.StartAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IParameterizedService.StartAsync)))
            .Returns(task);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartAsync(a, b, c);

        serviceMock.Verify(x => x.StartAsync(a, b, c), Times.Once);

        Assert.That(result, Is.SameAs(task));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessAsync),
                        nameof(IParameterizedService.StartAsync),
                        nameof(Interceptor.ProcessAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedTask = new Task<object>(() => new object());

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .Setup(x => x.TryStartAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IParameterizedService.TryStartAsync)))
            .Returns(expectedTask);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartAsync(a, b, c);

        serviceMock.Verify(x => x.TryStartAsync(a, b, c), Times.Once);

        Assert.That(result, Is.SameAs(expectedTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeAsync),
                        nameof(IParameterizedService.TryStartAsync),
                        nameof(Interceptor.ProcessWithReturnTypeAsync)
                    }));
    }
#if NETCOREAPP2_0_OR_GREATER

    [Test]
    public static void ProcessValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask(new Task(() => { }));

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .Setup(x => x.StartValueAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IParameterizedService.StartValueAsync)))
            .Returns(expectedValueTask);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartValueAsync(a, b, c);

        serviceMock.Verify(x => x.StartValueAsync(a, b, c), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessValueTaskAsync),
                        nameof(IParameterizedService.StartValueAsync),
                        nameof(Interceptor.ProcessValueTaskAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask<object>(new Task<object>(() => new object()));

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .Setup(x => x.TryStartValueAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IParameterizedService.TryStartValueAsync)))
            .Returns(expectedValueTask);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartValueAsync(a, b, c);

        serviceMock.Verify(x => x.TryStartValueAsync(a, b, c), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync),
                        nameof(IParameterizedService.TryStartValueAsync),
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync)
                    }));
    }
#endif

    [Test]
    public static void PropertyWithGetter()
    {
        var expectedResult = new object();

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .SetupGet(x => x.PropertyWithGetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(null!))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.PropertyWithGetter;

        Assert.That(result, Is.SameAs(expectedResult));
    }

    [Test]
    public static void PropertyWithSetter()
    {
        var someValue = new object();

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .SetupSet(x => x.PropertyWithSetter = someValue);

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(null!))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        decoratedInstance.PropertyWithSetter = someValue;

        serviceMock.VerifySet(x => x.PropertyWithSetter = someValue, Times.Once);
    }

    [Test]
    public static void PropertyWithGetterAndSetter()
    {
        var someValue = new object();
        var expectedResult = new object();

        var serviceMock = new Mock<IParameterizedService>();
        serviceMock
            .SetupSet(x => x.PropertyWithGetterAndSetter = someValue);
        serviceMock
            .SetupGet(x => x.PropertyWithGetterAndSetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(null!))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.PropertyWithGetterAndSetter;

        Assert.That(result, Is.SameAs(expectedResult));

        decoratedInstance.PropertyWithGetterAndSetter = someValue;

        serviceMock.VerifySet(x => x.PropertyWithGetterAndSetter = someValue, Times.Once);
    }
}
