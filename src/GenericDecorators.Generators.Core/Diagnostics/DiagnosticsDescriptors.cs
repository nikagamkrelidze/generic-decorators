using System;
using Microsoft.CodeAnalysis;

namespace GenericDecorators.Generators.Core.Diagnostics;

internal sealed class DiagnosticsDescriptors
{
    private const string Category = "Decorators";

    public static DiagnosticDescriptor InvalidInterfaceSymbol { get; } = CreateDiagnosticsDescriptor(
        id: "GGD1001",
        title: DiagnosticMessages.InvalidInterfaceSymbolTitle,
        messageFormat: DiagnosticMessages.InvalidInterfaceSymbolTitle,
        category: Category);

    public static DiagnosticDescriptor InvalidInterceptorSymbol { get; } = CreateDiagnosticsDescriptor(
        id: "GGD1002",
        title: DiagnosticMessages.InvalidInterceptorSymbolTitle,
        messageFormat: DiagnosticMessages.InvalidInterceptorSymbolMessage,
        category: Category);

    public static DiagnosticDescriptor NoRefParamteresAllowed { get; } = CreateDiagnosticsDescriptor(
        id: "GGD1003",
        title: DiagnosticMessages.NoRefParamteresAllowedTitle,
        messageFormat: DiagnosticMessages.NoRefParamteresAllowedMessage,
        category: Category);

    public static DiagnosticDescriptor NoParamsParamteresAllowed { get; } = CreateDiagnosticsDescriptor(
        id: "GGD1004",
        title: DiagnosticMessages.NoParamsParamteresAllowedTitle,
        messageFormat: DiagnosticMessages.NoParamsParamteresAllowedMessage,
        category: Category);

    public static DiagnosticDescriptor OnlyMethodsAndPropertiesAllowed { get; } = CreateDiagnosticsDescriptor(
        id: "GGD1005",
        title: DiagnosticMessages.OnlyMethodsPropertiesAllowedTitle,
        messageFormat: DiagnosticMessages.OnlyMethodsAndPropertiesAllowedMessage,
        category: Category);

    public static DiagnosticDescriptor NoOpenGenericInterfacesAllowed { get; } = CreateDiagnosticsDescriptor(
        id: "GGD1006",
        title: DiagnosticMessages.NoOpenGenericInterfacesAllowedTitle,
        messageFormat: DiagnosticMessages.NoOpenGenericInterfacesAllowedMessage,
        category: Category);
    
    private static DiagnosticDescriptor CreateDiagnosticsDescriptor(
        string id,
        string title,
        string messageFormat,
        string category,
        DiagnosticSeverity defaultSeverity = DiagnosticSeverity.Error,
        bool isEnabledByDefault = true)
    {
        return new DiagnosticDescriptor(
            id,
            title,
            messageFormat,
            category,
            defaultSeverity,
            isEnabledByDefault,
            null,
            null,
            Array.Empty<string>());
    }
}
