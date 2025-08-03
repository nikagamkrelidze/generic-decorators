Generic Decorators
===

A library for generating blazing fast [decorators](https://refactoring.guru/design-patterns/decorator/csharp/example) for arbitrary interfaces.

```csharp
// An example using factory pattern

MyInterceptor myInterceptor = new MyInterceptor();

IService serviceImplementation = new Service();

IService decoratedInstance = Decorator
    .For<IService, MyInterceptor>()
    .WithImplementation(serviceImplementation)
    .WithInterceptor(myInterceptor)
    .Instantiate();

decoratedInstance.DoSomething();

// Outputs:
// Hello from the MyInterceptor!
// Hello from the Service!
// Hello from the MyInterceptor again!
```

The Decoraor pattern is elegant and powerfull, but due to C#'s relatively strict type system, creating a single "interceptor" that encapsulates the decorating logic and reusing it for decorating multiple interfaces can be a very verbose.

The Generic Decorators library will generate a Decorator for any user-specified pair of types, an interface and an interceptor, using [source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/). The rich Roslyn APIs allow a thorough examination of the interface and interceptor types, and emit a tailored implementation with minimal overhead in compile-time.

[Click here](tba) for a complete documentation.

## Installation

### Factory Pattern

The Generic Decorators library is currently available for usage via Factory pattern. The Nuget package required for it to work is [GenericDecorators.Fluent](tba).

#### Package Manager Console

```
Install-Package GenericDecorators.Fluent
```

#### .NET Core CLI

```
dotnet add package GenericDecorators.Fluent
```