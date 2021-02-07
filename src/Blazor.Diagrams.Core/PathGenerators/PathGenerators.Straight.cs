using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Text;

namespace Blazor.Diagrams.Core
{
    public static partial class PathGenerators
    {
        public static string Straight(DiagramManager _, LinkModel link, Point[] route)
        {
            var sb = new StringBuilder(FormattableString.Invariant($"M {route[0].X} {route[0].Y}"));

            for (var i = 1; i < route.Length; i++)
            {
                sb.Append(FormattableString.Invariant($" L {route[i].X} {route[i].Y}"));
            }

            return sb.ToString();
        }
    }
}
