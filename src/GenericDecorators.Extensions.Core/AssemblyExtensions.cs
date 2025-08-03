using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GenericDecorators.Extensions.Core;

public static class AssemblyExtensions
{
    public static T[] InstantiateImplementationsOf<T>(this Assembly assembly)
    {
        var types = new List<Type>();

        try
        {
            types.AddRange(assembly.GetTypes());
        }
        catch (ReflectionTypeLoadException e)
        {
            types.AddRange(e.Types.Where(x => x != null).Select(x => x!));
        }

        return types
            .Where(x => x.GetInterfaces().Contains(typeof(T)) && x.GetConstructor(Type.EmptyTypes) != null)
            .Select(t => (T)Activator.CreateInstance(t)!)
            .ToArray();
    }
}