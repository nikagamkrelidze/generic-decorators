using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GenericDecorators.Extensions.Fluent;
using Moq;
using NUnit.Framework;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.ClosedGeneric;

public abstract class ClosedGenericSimpleInterceptorTestsBase
{
    protected abstract IClosedGenericService<Type1, Type2>? Instantiate(IClosedGenericService<Type1, Type2> internalImplementation, Interceptor interceptor);

    [Test]
    public void Process()
    {
        var invocations = new ConcurrentQueue<string>();

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .Setup(x => x.Start(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IClosedGenericService<Type1, Type2>.Start)));

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Instantiate(serviceMock.Object, new Interceptor(invocations));

        Assert.NotNull(decoratedInstance);

        decoratedInstance.Start(a, b, c);

        serviceMock.Verify(x => x.Start(a, b, c), Times.Once);

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.Process),
                        nameof(IClosedGenericService<Type1, Type2>.Start),
                        nameof(Interceptor.Process)
                    }));
    }

    [Test]
    public void ProcessWithReturnType()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedResult = new object();

        var a = new object();
        var b = new object();
        var c = new object();

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .Setup(x => x.TryStart(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IClosedGenericService<Type1, Type2>.TryStart)))
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
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
                        nameof(IClosedGenericService<Type1, Type2>.TryStart),
                        nameof(Interceptor.ProcessWithReturnType)
                    }));
    }

    [Test]
    public void ProcessAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var task = new Task(() => { });

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .Setup(x => x.StartAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IClosedGenericService<Type1, Type2>.StartAsync)))
            .Returns(task);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
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
                        nameof(IClosedGenericService<Type1, Type2>.StartAsync),
                        nameof(Interceptor.ProcessAsync)
                    }));
    }

    [Test]
    public void ProcessWithReturnTypeAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedTask = new Task<object>(() => new object());

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .Setup(x => x.TryStartAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IClosedGenericService<Type1, Type2>.TryStartAsync)))
            .Returns(expectedTask);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
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
                        nameof(IClosedGenericService<Type1, Type2>.TryStartAsync),
                        nameof(Interceptor.ProcessWithReturnTypeAsync)
                    }));
    }
#if NETCOREAPP2_0_OR_GREATER

    [Test]
    public void ProcessValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask(new Task(() => { }));

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .Setup(x => x.StartValueAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IClosedGenericService<Type1, Type2>.StartValueAsync)))
            .Returns(expectedValueTask);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
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
                        nameof(IClosedGenericService<Type1, Type2>.StartValueAsync),
                        nameof(Interceptor.ProcessValueTaskAsync)
                    }));
    }

    [Test]
    public void ProcessWithReturnTypeValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask<object>(new Task<object>(() => new object()));

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .Setup(x => x.TryStartValueAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>()))
            .Callback(() => invocations.Enqueue(nameof(IClosedGenericService<Type1, Type2>.TryStartValueAsync)))
            .Returns(expectedValueTask);

        var a = new object();
        var b = new object();
        var c = new object();

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
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
                        nameof(IClosedGenericService<Type1, Type2>.TryStartValueAsync),
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync)
                    }));
    }
#endif

    [Test]
    public void PropertyWithGetter()
    {
        var expectedResult = new object();

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .SetupGet(x => x.PropertyWithGetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(null!))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.PropertyWithGetter;

        Assert.That(result, Is.SameAs(expectedResult));
    }

    [Test]
    public void PropertyWithSetter()
    {
        var someValue = new object();

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .SetupSet(x => x.PropertyWithSetter = someValue);

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(null!))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        decoratedInstance.PropertyWithSetter = someValue;

        serviceMock.VerifySet(x => x.PropertyWithSetter = someValue, Times.Once);
    }

    [Test]
    public void PropertyWithGetterAndSetter()
    {
        var someValue = new object();
        var expectedResult = new object();

        var serviceMock = new Mock<IClosedGenericService<Type1, Type2>>();
        serviceMock
            .SetupSet(x => x.PropertyWithGetterAndSetter = someValue);
        serviceMock
            .SetupGet(x => x.PropertyWithGetterAndSetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<IClosedGenericService<Type1, Type2>, Interceptor>()
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

public class Type1
{
}

public class Type2 : Type1
{
}
