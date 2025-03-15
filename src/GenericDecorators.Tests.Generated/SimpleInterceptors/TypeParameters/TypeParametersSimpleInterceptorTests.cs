using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GenericDecorators.Extensions.Fluent;
using Moq;
using NUnit.Framework;

namespace GenericDecorators.Tests.Generated.SimpleInterceptors.TypeParameters;

public static class TypeParametersSimpleInterceptorTests
{
    [Test]
    public static void Process()
    {
        var invocations = new ConcurrentQueue<string>();

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock.Setup(x => x.Start<It.IsAnyType, It.IsAnyType, It.IsAnyType>(It.IsAny<object>(), It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>()))
            .Callback(() => invocations.Enqueue(nameof(ITypeParametersService.Start)));

        var a = new object();
        var b = new Type2();
        var c = new Type3();

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        decoratedInstance.Start<Type1, Type2, Type3>(a, b, c);

        serviceMock.Verify(x => x.Start<Type1, Type2, Type3>(a, b, c), Times.Once);

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.Process),
                        nameof(ITypeParametersService.Start),
                        nameof(Interceptor.Process)
                    }));
    }

    [Test]
    public static void ProcessWithReturnType()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedResult = new TypeResult();

        var a = new object();
        var b = new Type2();
        var c = new Type3();

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .Setup(x => x.TryStart<It.IsAnyType, It.IsAnyType, It.IsAnyType, TypeResult>(It.IsAny<object>(), It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>()))
            .Callback(() => invocations.Enqueue(nameof(ITypeParametersService.TryStart)))
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStart<Type1, Type2, Type3, TypeResult>(a, b, c);

        serviceMock.Verify(x => x.TryStart<Type1, Type2, Type3, TypeResult>(a, b, c), Times.Once);

        Assert.That(result, Is.SameAs(expectedResult));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnType),
                        nameof(ITypeParametersService.TryStart),
                        nameof(Interceptor.ProcessWithReturnType)
                    }));
    }

    [Test]
    public static void ProcessAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var task = new Task(() => { });

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .Setup(x => x.StartAsync<It.IsAnyType, It.IsAnyType, It.IsAnyType>(It.IsAny<object>(), It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>()))
            .Callback(() => invocations.Enqueue(nameof(ITypeParametersService.StartAsync)))
            .Returns(task);

        var a = new object();
        var b = new Type2();
        var c = new Type3();

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartAsync<Type1, Type2, Type3>(a, b, c);

        serviceMock.Verify(x => x.StartAsync<Type1, Type2, Type3>(a, b, c), Times.Once);

        Assert.That(result, Is.SameAs(task));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessAsync),
                        nameof(ITypeParametersService.StartAsync),
                        nameof(Interceptor.ProcessAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedTask = new Task<TypeResult>(() => new TypeResult());

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .Setup(x => x.TryStartAsync<It.IsAnyType, It.IsAnyType, It.IsAnyType, TypeResult>(It.IsAny<object>(), It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>()))
            .Callback(() => invocations.Enqueue(nameof(ITypeParametersService.TryStartAsync)))
            .Returns(expectedTask);

        var a = new object();
        var b = new Type2();
        var c = new Type3();

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartAsync<Type1, Type2, Type3, TypeResult>(a, b, c);

        serviceMock.Verify(x => x.TryStartAsync<Type1, Type2, Type3, TypeResult>(a, b, c), Times.Once);

        Assert.That(result, Is.SameAs(expectedTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeAsync),
                        nameof(ITypeParametersService.TryStartAsync),
                        nameof(Interceptor.ProcessWithReturnTypeAsync)
                    }));
    }
#if NETCOREAPP2_0_OR_GREATER

    [Test]
    public static void ProcessValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask(new Task(() => { }));

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .Setup(x => x.StartValueAsync<It.IsAnyType, It.IsAnyType, It.IsAnyType>(It.IsAny<object>(), It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>()))
            .Callback(() => invocations.Enqueue(nameof(ITypeParametersService.StartValueAsync)))
            .Returns(expectedValueTask);

        var a = new object();
        var b = new Type2();
        var c = new Type3();

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.StartValueAsync<Type1, Type2, Type3>(a, b, c);

        serviceMock.Verify(x => x.StartValueAsync<Type1, Type2, Type3>(a, b, c), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessValueTaskAsync),
                        nameof(ITypeParametersService.StartValueAsync),
                        nameof(Interceptor.ProcessValueTaskAsync)
                    }));
    }

    [Test]
    public static void ProcessWithReturnTypeValueTaskAsync()
    {
        var invocations = new ConcurrentQueue<string>();

        var expectedValueTask = new ValueTask<TypeResult>(new Task<TypeResult>(() => new TypeResult()));

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .Setup(x => x.TryStartValueAsync<It.IsAnyType, It.IsAnyType, It.IsAnyType, TypeResult>(It.IsAny<object>(), It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>()))
            .Callback(() => invocations.Enqueue(nameof(ITypeParametersService.TryStartValueAsync)))
            .Returns(expectedValueTask);

        var a = new object();
        var b = new Type2();
        var c = new Type3();

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(invocations))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.TryStartValueAsync<Type1, Type2, Type3, TypeResult>(a, b, c);

        serviceMock.Verify(x => x.TryStartValueAsync<Type1, Type2, Type3, TypeResult>(a, b, c), Times.Once);

        Assert.That(result, Is.EqualTo(expectedValueTask));

        Assert.True(
            invocations
                .SequenceEqual(
                    new[]
                    {
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync),
                        nameof(ITypeParametersService.TryStartValueAsync),
                        nameof(Interceptor.ProcessWithReturnTypeValueTaskAsync)
                    }));
    }
#endif

    [Test]
    public static void PropertyWithGetter()
    {
        var expectedResult = new object();

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .SetupGet(x => x.PropertyWithGetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
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

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .SetupSet(x => x.PropertyWithSetter = someValue);

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
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

        var serviceMock = new Mock<ITypeParametersService>();
        serviceMock
            .SetupSet(x => x.PropertyWithGetterAndSetter = someValue);
        serviceMock
            .SetupGet(x => x.PropertyWithGetterAndSetter)
            .Returns(expectedResult);

        var decoratedInstance = Decorator
            .For<ITypeParametersService, Interceptor>()
            .WithImplementation(serviceMock.Object)
            .WithInterceptor(new Interceptor(null!))
            .Instantiate();

        Assert.NotNull(decoratedInstance);

        var result = decoratedInstance.PropertyWithGetterAndSetter;

        Assert.That(result, Is.SameAs(expectedResult));

        decoratedInstance.PropertyWithGetterAndSetter = someValue;

        serviceMock.VerifySet(x => x.PropertyWithGetterAndSetter = someValue, Times.Once);
    }

    private class Type1
    {
    }

    private class Type2
    {
    }

    private class Type3 : Type2
    {
    }

    private class TypeResult
    {
    }
}
