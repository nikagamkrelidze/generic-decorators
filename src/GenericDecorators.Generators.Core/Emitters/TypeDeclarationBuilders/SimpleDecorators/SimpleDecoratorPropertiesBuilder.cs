using System;
using System.Collections.Generic;
using GenericDecorators.Generators.Core.Parsers.Contexts.Primitives;

namespace GenericDecorators.Generators.Core.Emitters.TypeDeclarationBuilders.SimpleDecorators;

internal static class SimpleDecoratorPropertiesBuilder
{
    public static string GeneratePropertyFromContext(
        PropertyContext context,
        string interfaceName,
        string underlyingImplementationFieldName,
        string notImplementedExceptionFullyQualifiedName)
    {
        var accessors = new List<string>();

        foreach (PropertyAccessors accessor in context.AccessModifiers)
        {
            switch (accessor)
            {
                case PropertyAccessors.Get:
                    var getter =
                        $$"""
                          get
                                  {
                                      return {{underlyingImplementationFieldName}}.{{context.Name}};
                                  }
                          """;
                    accessors.Add(getter);
                    break;
                case PropertyAccessors.Set:
                    var setter =
                        $$"""
                          set
                                  {
                                      {{underlyingImplementationFieldName}}.{{context.Name}} = value;
                                  }
                          """;
                    accessors.Add(setter);
                    break;
                case PropertyAccessors.Init:
                    var init =
                        $$"""
                          init
                                  {
                                      throw new {{notImplementedExceptionFullyQualifiedName}}();
                                  }
                          """;
                    accessors.Add(init);
                    break;
            }
        }

        return $$"""
                 
                     {{context.Type.FullyQualifiedName}} {{interfaceName}}.{{context.Name}}
                     {
                         {{string.Join(Environment.NewLine, accessors)}}
                     }
                 """;
    }
}
