using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Extensions;

internal static class MethodSymbolExtensions
{
    public static InterceptorKind GetInterceptorKind(this IMethodSymbol methodSymbol) =>
        (methodSymbol.ReturnType as INamedTypeSymbol) switch
        {
            { SpecialType: SpecialType.System_Void } => InterceptorKind.Void,
            { IsGenericType: true, Name: nameof(Task), TypeArguments.Length: 1 } => InterceptorKind.TaskGeneric,
            { IsReferenceType: true, Name: nameof(Task) } => InterceptorKind.Task,
            { IsGenericType: true, Name: nameof(ValueTask), TypeArguments.Length: 1 } => InterceptorKind.ValueTaskGeneric,
            { Name: nameof(ValueTask) } => InterceptorKind.ValueTask,
            _ => InterceptorKind.Value
        };

    public static bool IsAwaitable(this IMethodSymbol methodSymbol) => methodSymbol.GetInterceptorKind() switch
    {
        InterceptorKind.Void => false,
        InterceptorKind.Value => false,
        _ => true
    };

    public static bool IsInterceptor(this IMethodSymbol methodSymbol, INamedTypeSymbol simpleInterceptorSymbol)
    {
        var methods = simpleInterceptorSymbol
            .GetMembers()
            .OfType<IMethodSymbol>();

        return methods.Any(x => methodSymbol.IsOverrideOf(x) || methodSymbol.Equals(x, SymbolEqualityComparer.Default));
    }

    private static bool IsOverrideOf(this IMethodSymbol methodSymbol, IMethodSymbol potentialBaseMethod)
    {
        if (!methodSymbol.IsOverride)
        {
            return false;
        }

        var currentOverriddenMethod = methodSymbol.OverriddenMethod;

        while (currentOverriddenMethod != null)
        {
            if (currentOverriddenMethod.Equals(potentialBaseMethod, SymbolEqualityComparer.Default))
            {
                return true;
            }

            currentOverriddenMethod = currentOverriddenMethod.OverriddenMethod;
        }

        return false;
    }
}

/// <summary>
/// The kind of interceptor, based on the return type of the underlying method.
/// </summary>
public enum InterceptorKind
{
    /// <summary>
    /// Corresponds to a method with a return type of <see cref="void"/>.
    /// </summary>
    Void,

    /// <summary>
    /// Corresponds to a method with any non-awaitable return type.
    /// </summary>
    Value,

    /// <summary>
    /// Corresponds to a method with a return type of <see cref="Task"/>/>.
    /// </summary>
    Task,

    /// <summary>
    /// Corresponds to a method with a return type of <see cref="Task{TResult}"/>/>.
    /// </summary>
    TaskGeneric,

    /// <summary>
    /// Corresponds to a method with a return type of <see cref="ValueTask"/>/>.
    /// </summary>
    ValueTask,

    /// <summary>
    /// Corresponds to a method with a return type of <see cref="ValueTask{TResult}"/>/>.
    /// </summary>
    ValueTaskGeneric
}