Welcome to for Generic Decorators!
===================================

**Generic Decorators** is a C# library for generating blazing fast `decorators <https://refactoring.guru/design-patterns/decorator/csharp/example>`_ for arbitrary interfaces.

While traditional approaches are either exremely verbose and error-prone (like writing individual decorators by hand) or slow and clumsy (using libraries commonly used for proxying,
like `Castle's legendary DynamicProxy <https://www.castleproject.org/projects/dynamicproxy/>`_), **Generic Decorators** rely on
`Roslyn's Source Generators <https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/>`_ for gnerating fast and efficient decorators durinc compile-time.

.. warning::

   This project is under active development.

Contents
--------

.. toctree::
   :maxdepth: 2

   getting_started/index
   user_guide/index
   contributing/index