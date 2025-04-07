Background and Motivation
=========================

Why decorators?
---------------

.. note::

   This is a general introduction to decorators. Feel free to skip it, if you are familiar with the concept.

We, as engineers, love identifying repetitive patterns, cross-cutting concerns, isolating and abstracting them -- and it's a good thing.
While there definitely can be too much of a good thing, some design patterns feel like they're just meant to be.

Decorators are a design pattern that allows us to encapsulate cross-cutting concerns and define them in terms of "behavioral" `aspects <https://en.wikipedia.org/wiki/Aspect-oriented_programming>`_
of *types*. If you're working on a C# project and you care about your code design, there's a good chance you have a bunch of different interfaces laying around.
It is quite common, to then need to apply some cross-cutting concern to some or all of those interfaces -- for example, log the names and the execution durations
of those interface implementations, or add some `circuit-breakers <https://learn.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker>`_.

Decorators achieve this by implementing the interface (or abstract class) but only to the extent of the concern that they are trying to address, deferring to the
actual implementation of the type after it has "done its thing".

For example, let's consider an interface ``ITemperatureMeter`` with some basic implementation ``RandomTemperatureMeter``:

.. sourcecode:: csharp
    :linenos:

    interface ITemperatureMeter
    {
        double GetTemperature();
    }

    class RandomTemperatureMeter : ITemperatureMeter
    {
        public double GetTemperature()
        {
            var t = Random.Shared.NextDouble() * 100;

            return t;
        }
    }


Say we want to extend the implementation by adding logging to the method ``GetTemperature``:

.. sourcecode:: csharp
    :linenos:
    
    class RandomTemperatureMeter : ITemperatureMeter
    {
        public double GetTemperature()
        {
            Console.WriteLine($"Starting to get temperature at {DateTime.UtcNow.Ticks}");
            
            var t = Random.Shared.NextDouble() * 100;
            
            Console.WriteLine($"Finished getting temperature at {DateTime.UtcNow.Ticks}");
            
            return t;
        }
    }

The above implementation achieves the set goal, however, there are a couple of caveates:

#. Our logging code and logic is now intertwined with the "business logic" of the method -- in this case, that's the part that does the job of measuring the temperature by generating
   a random number from 0 to 100 on line 9.
#. The logging code and logic will need to be replicated to other implementations of ``ITemperatureMeter`` and indeed, other interfaces willing to receive such functionality.

The issues with the above mentioned caveats will obviously multiply as we add more cross-cutting concerns and interfaces involved and given enough of them, they will
surely start causing issues -- another way of saying that this approach doesn't scale well.

Decorators help us address those issues to some extent.

By applying the decorator pattern, we can rewrite our example by introducing an implementation-agnostic ``LoggingTemperatureMeterDecorator`` in the following way:

.. _LoggingTemperatureMeterDecorator:
.. sourcecode:: csharp
    :linenos:

    class LoggingTemperatureMeterDecorator : ITemperatureMeter
    {
        private readonly ITemperatureMeter _internalImplementation;

        public LoggingTemperatureMeterDecorator(ITemperatureMeter internalImplementation)
        {
            _internalImplementation = internalImplementation;
        }

        public double GetTemperature()
        {
            Console.WriteLine($"Starting to get temperature at {DateTime.UtcNow.Ticks}");
            
            var t = _internalImplementation.GetTemperature();
            
            Console.WriteLine($"Finished getting temperature at {DateTime.UtcNow.Ticks}");
            
            return t;
        }
    }

    class RandomTemperatureMeter : ITemperatureMeter
    {
        public double GetTemperature()
        {
            var t = Random.Shared.NextDouble() * 100;
            
            return t;
        }
    }

Notice, how the newly-introduced type ``LoggingTemperatureMeterDecorator`` addresses the the logging *aspect* without being tied to any particular implementation of the ``ITemperatureMeter``.
It achieves this decoupling, by accepting any ``ITemperatureMeter`` implementation in its constructor and deferring to it in its implementation of the ``GetTemperature``, on line 14.
We still have ``RandomTemperatureMeter`` with the actual business logic, but it's back to it's trimmed down version and does not contain verbose and awkwardly placed concerns anymore.

The usage of such a set of types would look something like the following:

.. sourcecode:: csharp
    :linenos:

    ITemperatureMeter internalImplementation = new RandomTemperatureMeter();
    ITemperatureMeter temperatureMeter = new LoggingTemperatureMeterDecorator(internalImplementation);

    var temperature = temperatureMeter.GetTemperature();

    Console.WriteLine($"Temperature: {temperature}°C");

The obvious upside to this approach is that not only we're not mixing pieces of code with clearly different types of responsibilities (one being business logic and another being observability),
but this decoupling also allows us to apply the  ``LoggingTemperatureMeterDecorator`` to any other implementation of ``ITemperatureMeter``. There is nothing preventing us substituting the
``RandomTemperatureMeter`` with, say ``SomeOtherTemperatureMeter`` -- as long as it implements ``ITemperatureMeter`` and thus belongs to the same "family" of types, .NET type system is happy
and will allow us this flexibility.

We could, in fact, implement other kinds of decorators and nest them in the similar manner -- as long as they all implement the same interface that we're trying to decorate -- in this case ``ITemperatureMeter``.

While this approach goes a long way, there is one subtle but critical issue -- while we managed to extract the concern, isolate it and make it reusable, it's only really reusable for other types that also
implement the ``ITemperatureMeter``. It can only decorate other ``ITemperatureMeter`` implementations -- after all, what would it defer to in case we try to use it somehow for a type that doesn't have
a method with the signature ``public double GetTemperature()``?

This is the sense, in which that decorator is *not* generic, and is precisely what this library tries to solve.

The problem with .NET type system
---------------------------------

The problem with turning the ``LoggingTemperatureMeterDecorator`` from the :ref:`previous section <LoggingTemperatureMeterDecorator>` into a generic logging decorator that is able to accomodate
any given interface, lies in the fact that due to the constraints of the .NET type system, it needs to implement the interface that it's trying to decorate -- and that's required for multiple reasons.

It would be convenient to have some ``GenericLoggingDecorator``, which we would be able to use like following:

.. sourcecode:: csharp
    :linenos:

    IFirstInterface first = new GenericLoggingDecorator(new FirstImplementation());

    ISecondInterface second = new GenericLoggingDecorator(new SecondImplementation());

However, there are several problems with that.

Firstly, it clearly implies that the ``GenericLoggingDecorator`` needs to be both, ``IFirstInterface`` and ``ISecondInterface``. While technically there is nothing wrong with that, it definitely
does not scale, as we would need to add explicit implementations of all the interfaces that we would want to use our decorator with, and that is far worse than having to write separate per-interface
decorators by hand, which is what we are trying to avoid by trying to make ``GenericLoggingDecorator`` truly generic.

Secondly and obviously, it's not really generic if we have to write individual implementations for each new interface.

This has been a known quirk of the strongly-typed languages for a long time. In fact, a common usage of decorators in Python looks like this:

.. sourcecode:: python
    :linenos:

    def my_decorator(func):
        def wrapper(*args, **kwargs):
            print("Before call")
            result = func(*args, **kwargs)  # Call the original function
            print("After call")
            return result
        return wrapper

    @my_decorator
    def greet(name):
        print(f"Hello, {name}!")

    greet("Alice")

Notice, how ``my_decorator`` does not care about the signature of the ``greet`` function -- it is in fact able to decorate any function that you apply it to, even if that function doesn't
return any value (even though the line 4 implies that it does) -- a detail which wouldn't go unnoticed in C#.


Existing solutions
------------------

As mentioned in the :ref:`previous section <The problem with .NET type system>`, the constraints of strict type systems (when it comes to trying to make decorators and
`proxies <https://refactoring.guru/design-patterns/proxy>`_ generic) are well known and had been addressed to some extent in C#.

The main idea behind most popular approaches is simple: to apply the decorating logic to multiple interfaces, we need a custom implementation of decorators tailored to each
interface and its members. However, the decorators themselves -- meaning their actual definitions as seen "in code" -- are very *predictable*. They are predictable in the sense, that
given two pieces of of code, a "decorating logic" (encapsulated in what is commonly called an **interceptor**) and an interface that we want to decorate, it's likely there is a single
most efficient way to produce a decorator type. It would simply need to "implement a given interface, accept an actual, real implementation in a constructor and defer to it, apply
interceptor before and after invoking the actual, real implementation". That's pretty deterministic.

This means that it is well possible to use some reflection-based means available in .NET/C# in order to programatically inspect the "decorating logic" and an interface and *emit* the definition
of a resulting decorator. A popular way to do it, is through a set of types defined in ``System.Reflection.Emit``- through runtime or
`"dynamic" emission <https://learn.microsoft.com/en-us/dotnet/fundamentals/reflection/emitting-dynamic-methods-and-assemblies>`_.

Indeed, the most popular library that achieves generic decoration, `Castle's DynamicProxy <https://www.castleproject.org/projects/dynamicproxy/>`_ does exactly that.

.. note::

   Proxies and decorators are two distinct but somewhat related patterns. Libraries, such as above mentioned `DynamicProxy <https://www.castleproject.org/projects/dynamicproxy/>`_ can be used to achieve both.

The ``DynamicProxy`` is the powerhouse behind a lot of popular .NET libraries, such as `Autofac <https://autofac.readthedocs.io/en/latest/advanced/interceptors.html>`_,
`Moq <https://github.com/devlooped/moq?tab=readme-ov-file#who>`_ and many others. It's safe to say that it's a standard when it comes to seemlessly injecting interceptors to achieve proxying and decoration.

However, there are couple of caveates to using this approach.

Firstly, because ``DynamicProxy`` operates with .NET concepts, intercepting awaitable methods and following up with some work after they are finished can be a little awkward -- at the moment of writing,
async/await exist on purely a C# layer. Because the .NET method that the asyncronous C# method compiles into, (almost always) really finishes execution upon the first usage of the ``await``, ``DynamicProxy``
sees that as a time to kick its post-execution logic in. While that's technically correct from the .NET perspective, the C# method that we wanted to intercept and decorate hasn't necessarily finished
execution at that time. There are workarounds for this issue, but this demonstrates how because ``DynamicProxy`` operates on a different level, it can be quite inefficient, leading to the topic of performance.

There is a lot of overhead associated with ``DynamicProxy``-based interception and decoration. The `benchmarks <TBA>`_ included in the **Generic Decorators** project demonstrate the shortcomings of the approach,
both in speed and memory allocations. This makes the ``DynamicProxy``-based decoration not suitable for high-performance scenarios.

The approach taken by **Generic Decorators** (and more specifically, the *kinds* of decorators that the library produces, with the techniques that address common pitfalls) makes sure that the overheads are kept
to a minimum and achieves *near-perfect results*, compared to the decorators written manually. For more details, refer to the section on `benchmarking <tba>`_.