using System.Text;

namespace Blazor.Diagrams.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendIf(this StringBuilder builder, string str, bool condition)
    {
        if (condition) builder.Append(str);

        return builder;
    }
}