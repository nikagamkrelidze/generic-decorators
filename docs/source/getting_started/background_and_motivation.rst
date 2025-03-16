Background and Motivation
=========================

Why decorators?
---------------

We, as engineers, love identifying repetitive patterns, "cross-cutting concerns", isolating and abstracting them - and it's a good thing.
While there definitely can be too much of a good thing, some design patterns feel like they're just meant to be.

Decorators allow us to encapsulate cross-cutting concerns and define them in terms of "behavioral" `aspects <https://en.wikipedia.org/wiki/Aspect-oriented_programming>`_ of *types*.
If you're working on a C# project and you care about your code design, there's a good chance you have a bunch of different interfaces laying around.
It is quite common, to then need to apply some cross-cutting concern to some or all of those interfaces - for example, log the names and the execution durations
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

#. Our logging code and logic is now intertwined with the "business logic" of the method - in this case, that's the part that does the job of measuring the temperature by generating a random number from 0 to 100 on line 9.
#. The logging code and logic will likely need to be replicated to other implementations of ``ITemperatureMeter`` and indeed, other interfaces willing to receive such functionality.

The issues with the above mentioned caveats will obviously multiply as we add more cross-cutting concerns and interfaces involved and given enough of them, they will
surely start causing issues - another way of saying that this approach doesn't scale well.