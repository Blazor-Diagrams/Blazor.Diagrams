using System.Globalization;

namespace Blazor.Diagrams.Core.Extensions;

public static class NumberExtensions
{
    public static string ToInvariantString(this double n) => n.ToString(CultureInfo.InvariantCulture);
}
