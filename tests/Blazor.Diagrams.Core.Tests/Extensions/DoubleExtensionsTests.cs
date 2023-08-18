using Xunit;
using Blazor.Diagrams.Core.Extensions;

namespace Blazor.Diagrams.Core.Tests.Extensions;

public class DoubleExtensionsTests
{
    [Theory]
    [InlineData(5, 10, 0.1, false)]
    [InlineData(1.1, 1.2, 0.01, false)]
    [InlineData(10, 10, 0.0001, true)]
    [InlineData(10.35, 10.35, 0.0001, true)]
    [InlineData(1.659, 1.660, 0.0001, false)]
    [InlineData(1.65999, 1.65998, 0.0001, true)]
    [InlineData(1.65999, 1.6599998, 0.0001, true)]
    public void AlmostEqualTo(double num1, double num2, double tolerance, bool expected)
    {
        Assert.Equal(expected, num1.AlmostEqualTo(num2, tolerance));
    }
}
