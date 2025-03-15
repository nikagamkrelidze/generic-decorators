using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GenericDecorators.Extensions.Fluent;
using Moq;
using NUnit.Framework;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.Parameterless;

public static class ParameterlessSimpleInterceptorTests
{
    [Test]
    public static void Process()
    {
        var invocations = new ConcurrentQueue<string>();

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock.Setup(x => x.Start()).Callback(() => invocations.Enqueue(nameof(IParameterlessService.Start)));

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        decoratedInstance.Start();

        serviceMock.Verify(x => x.Start(), Times.Once);

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.Process),
                        nameof(IParameterlessService.Start),
                        nameof(Interceptor.Process)
                    }));
    }
    
    [Test]
    public static void ProcessWithReturnType()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedResult = new object();

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .Setup(x => x.TryStart())
            .Callback(() => invocations.Enqueue(nameof(IParameterlessService.TryStart)))
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStart();

        serviceMock.Verify(x => x.TryStart(), Times.Once);

        Assert.That(result, Is.SameAs(expectedResult));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnType),
                        nameof(IParameterlessService.TryStart),
                        nameof(Interceptor.ProcessWithReturnType)
                    }));
    }

    [Test]
    public static void ProcessAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var task = new Task(() => { });

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .Setup(x => x.StartAsync())
            .Callback(() => invocations.Enqueue(nameof(IParameterlessService.StartAsync)))
            .Returns(task);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartAsync();

        serviceMock.Verify(x => x.StartAsync(), Times.Once);

        Assert.That(result, Is.SameAs(task));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessAsync),
                        nameof(IParameterlessService.StartAsync),
                        nameof(Interceptor.ProcessAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedTask = new Task<object>(() => new object());

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .Setup(x => x.TryStartAsync())
            .Callback(() => invocations.Enqueue(nameof(IParameterlessService.TryStartAsync)))
            .Returns(expectedTask);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartAsync();

        serviceMock.Verify(x => x.TryStartAsync(), Times.Once);

        Assert.That(result, Is.SameAs(expectedTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeAsync),
                        nameof(IParameterlessService.TryStartAsync),
                        nameof(Interceptor.ProcessWithReturnTypeAsync)
                    }));
    }
#if NETCOREAPP2_0_OR_GREATER

    [Test]
    public static void ProcessValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask(new Task(() => { }));

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .Setup(x => x.StartValueAsync())
            .Callback(() => invocations.Enqueue(nameof(IParameterlessService.StartValueAsync)))
            .Returns(expectedValueTask);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartValueAsync();

        serviceMock.Verify(x => x.StartValueAsync(), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessValueTaskAsync),
                        nameof(IParameterlessService.StartValueAsync),
                        nameof(Interceptor.ProcessValueTaskAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask<object>(new Task<object>(() => new object()));

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .Setup(x => x.TryStartValueAsync())
            .Callback(() => invocations.Enqueue(nameof(IParameterlessService.TryStartValueAsync)))
            .Returns(expectedValueTask);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartValueAsync();

        serviceMock.Verify(x => x.TryStartValueAsync(), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync),
                        nameof(IParameterlessService.TryStartValueAsync),
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync)
                    }));
    }

#endif

    [Test]
    public static void PropertyWithGetter()
    {
        var expectedResult = new object();

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .SetupGet(x => x.PropertyWithGetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
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

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .SetupSet(x => x.PropertyWithSetter = someValue);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
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

        var serviceMock = new Mock<IParameterlessService>();
        serviceMock
            .SetupSet(x => x.PropertyWithGetterAndSetter = someValue);
        serviceMock
            .SetupGet(x => x.PropertyWithGetterAndSetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IParameterlessService, Interceptor>()
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
