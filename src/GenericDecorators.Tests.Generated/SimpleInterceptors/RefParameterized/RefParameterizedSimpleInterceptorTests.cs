using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GenericDecorators.Extensions.Fluent;
using Moq;
using NUnit.Framework;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.RefParameterized;

public static class RefParameterizedSimpleInterceptorTests
{
    [Test]
    public static void Process()
    {
        var invocations = new ConcurrentQueue<string>();

        var bFromInternalImplementation = new object();
        var cFromInternalImplementation = new object();

        var serviceMock = new Mock<IRefParameterizedService>();

        serviceMock.Setup(x => x.Start(
            in It.Ref<object>.IsAny,
            ref It.Ref<object>.IsAny,
            out It.Ref<object>.IsAny,
            It.IsAny<object>()))
        .Callback(
            (in object _, ref object b, out object c, object _) =>
            {
                invocations.Enqueue(nameof(IRefParameterizedService.Start));

                b = bFromInternalImplementation;
                c = cFromInternalImplementation;
            });

        var a = new object();
        var b = new object();
        var d = new object();

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        decoratedInstance.Start(in a, ref b, out var c, d);

        serviceMock.Verify(x => x.Start(in a, ref bFromInternalImplementation, out cFromInternalImplementation, d), Times.Once);

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.Process),
                        nameof(IRefParameterizedService.Start),
                        nameof(Interceptor.Process)
                    }));
    }

    [Test]
    public static void ProcessWithReturnType()
    {
        var invocations = new ConcurrentQueue<string>();

        var bFromInternalImplementation = new object();
        var cFromInternalImplementation = new object();

        var serviceMock = new Mock<IRefParameterizedService>();

        var expectedResult = new object();

        serviceMock
            .Setup(x => x.TryStart(
                in It.Ref<object>.IsAny,
                ref It.Ref<object>.IsAny,
                out It.Ref<object>.IsAny,
                It.IsAny<object>()))
            .Callback(
                (in object _, ref object b, out object c, object _) =>
                {
                    invocations.Enqueue(nameof(IRefParameterizedService.TryStart));

                    b = bFromInternalImplementation;
                    c = cFromInternalImplementation;
                })
            .Returns(expectedResult);

        var a = new object();
        var b = new object();
        var d = new object();

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStart(in a, ref b, out var c, d);

        serviceMock.Verify(x => x.TryStart(in a, ref bFromInternalImplementation, out cFromInternalImplementation, d), Times.Once);

        Assert.That(result, Is.SameAs(expectedResult));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnType),
                        nameof(IRefParameterizedService.TryStart),
                        nameof(Interceptor.ProcessWithReturnType)
                    }));
    }

    [Test]
    public static void ProcessAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var bFromInternalImplementation = new object();
        var cFromInternalImplementation = new object();

        var task = new Task(() => { });

        var serviceMock = new Mock<IRefParameterizedService>();
        serviceMock
            .Setup(x => x.StartAsync(
                in It.Ref<object>.IsAny,
                ref It.Ref<object>.IsAny,
                out It.Ref<object>.IsAny,
                It.IsAny<object>()))
            .Callback(
                (in object _, ref object b, out object c, object _) =>
                {
                    invocations.Enqueue(nameof(IRefParameterizedService.StartAsync));

                    b = bFromInternalImplementation;
                    c = cFromInternalImplementation;
                })
            .Returns(task);

        var a = new object();
        var b = new object();
        var d = new object();

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartAsync(in a, ref b, out var c, d);

        serviceMock.Verify(x => x.StartAsync(in a, ref bFromInternalImplementation, out cFromInternalImplementation, d), Times.Once);

        Assert.That(result, Is.SameAs(task));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessAsync),
                        nameof(IRefParameterizedService.StartAsync),
                        nameof(Interceptor.ProcessAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var bFromInternalImplementation = new object();
        var cFromInternalImplementation = new object();

        var expectedTask = new Task<object>(() => new object());

        var serviceMock = new Mock<IRefParameterizedService>();
        serviceMock
            .Setup(x => x.TryStartAsync(
                in It.Ref<object>.IsAny,
                ref It.Ref<object>.IsAny,
                out It.Ref<object>.IsAny,
                It.IsAny<object>()))
            .Callback(
                (in object _, ref object b, out object c, object _) =>
                {
                    invocations.Enqueue(nameof(IRefParameterizedService.TryStartAsync));

                    b = bFromInternalImplementation;
                    c = cFromInternalImplementation;
                })
            .Returns(expectedTask);

        var a = new object();
        var b = new object();
        var d = new object();

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartAsync(in a, ref b, out var c, d);

        serviceMock.Verify(x => x.TryStartAsync(in a, ref bFromInternalImplementation, out cFromInternalImplementation, d), Times.Once);

        Assert.That(result, Is.SameAs(expectedTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeAsync),
                        nameof(IRefParameterizedService.TryStartAsync),
                        nameof(Interceptor.ProcessWithReturnTypeAsync)
                    }));
    }

#if NETCOREAPP2_0_OR_GREATER

    [Test]
    public static void ProcessValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var bFromInternalImplementation = new object();
        var cFromInternalImplementation = new object();

        var expectedValueTask = new ValueTask(new Task(() => { }));

        var serviceMock = new Mock<IRefParameterizedService>();
        serviceMock
            .Setup(x => x.StartValueAsync(
                in It.Ref<object>.IsAny,
                ref It.Ref<object>.IsAny,
                out It.Ref<object>.IsAny,
                It.IsAny<object>()))
            .Callback(
                (in object _, ref object b, out object c, object _) =>
                {
                    invocations.Enqueue(nameof(IRefParameterizedService.StartValueAsync));

                    b = bFromInternalImplementation;
                    c = cFromInternalImplementation;
                })
            .Returns(expectedValueTask);

        var a = new object();
        var b = new object();
        var d = new object();

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartValueAsync(in a, ref b, out var c, d);

        serviceMock.Verify(x => x.StartValueAsync(in a, ref bFromInternalImplementation, out cFromInternalImplementation, d), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessValueTaskAsync),
                        nameof(IRefParameterizedService.StartValueAsync),
                        nameof(Interceptor.ProcessValueTaskAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var bFromInternalImplementation = new object();
        var cFromInternalImplementation = new object();

        var expectedValueTask = new ValueTask<object>(new Task<object>(() => new object()));

        var serviceMock = new Mock<IRefParameterizedService>();
        serviceMock
            .Setup(x => x.TryStartValueAsync(
                in It.Ref<object>.IsAny,
                ref It.Ref<object>.IsAny,
                out It.Ref<object>.IsAny,
                It.IsAny<object>()))
            .Callback(
                (in object _, ref object b, out object c, object _) =>
                {
                    invocations.Enqueue(nameof(IRefParameterizedService.TryStartValueAsync));

                    b = bFromInternalImplementation;
                    c = cFromInternalImplementation;
                })
            .Returns(expectedValueTask);

        var a = new object();
        var b = new object();
        var d = new object();

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartValueAsync(in a, ref b, out var c, d);

        serviceMock.Verify(x => x.TryStartValueAsync(in a, ref bFromInternalImplementation, out cFromInternalImplementation, d), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync),
                        nameof(IRefParameterizedService.TryStartValueAsync),
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync)
                    }));
    }
#endif

    [Test]
    public static void PropertyWithGetter()
    {
        var expectedResult = new object();

        var serviceMock = new Mock<IRefParameterizedService>();
        serviceMock
            .SetupGet(x => x.PropertyWithGetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
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

        var serviceMock = new Mock<IRefParameterizedService>();
        serviceMock
            .SetupSet(x => x.PropertyWithSetter = someValue);

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
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

        var serviceMock = new Mock<IRefParameterizedService>();
        serviceMock
            .SetupSet(x => x.PropertyWithGetterAndSetter = someValue);
        serviceMock
            .SetupGet(x => x.PropertyWithGetterAndSetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IRefParameterizedService, Interceptor>()
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
