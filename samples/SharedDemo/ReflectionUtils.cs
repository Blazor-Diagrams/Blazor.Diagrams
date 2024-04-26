using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SharedDemo;

public static class ReflectionUtils
{
    public static IEnumerable<PossibleOption> ExtractPossibleOptions<T>()
    {
        var type = typeof(T);
        return ExtractPossibleOptions(type, string.Empty, Activator.CreateInstance(type));
    }

    private static IEnumerable<PossibleOption> ExtractPossibleOptions(Type type, string prefix, object instance)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var name = $"{prefix}{property.Name}";
            var propertyValue = instance == null ? null : property.GetValue(instance);

            if (!IsPrimitiveOrNullable(property.PropertyType))
            {
                foreach (var entry in ExtractPossibleOptions(property.PropertyType, name + ".", propertyValue))
                    yield return entry;

                continue;
            }

            var typeName = FormatPropertyType(property.PropertyType);
            var @default = propertyValue?.ToString();
            var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description;
            yield return new PossibleOption(name, typeName, @default, description);
        }
    }

    private static string FormatPropertyType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            return $"{type.GetGenericArguments()[0].Name}?";

        return type.Name;
    }

    private static bool IsPrimitiveOrNullable(Type type)
    {
        return type == typeof(object) ||
            type == typeof(Type) ||
            Type.GetTypeCode(type) != TypeCode.Object ||
            Nullable.GetUnderlyingType(type) != null ||
            typeof(Delegate).IsAssignableFrom(type);
    }
}

public class PossibleOption
{
    public string Name { get; }
    public string Type { get; }
    public string Default { get; }
    public string Description { get; }

    public PossibleOption(string name, string type, string @default, string description)
    {
        Name = name;
        Type = type;
        Default = @default;
        Description = description;
    }
}
