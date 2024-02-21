using Blazor.Diagrams.Core.Geometry;
using Microsoft.JSInterop;
using Moq;
using System.Threading.Tasks;
using Xunit;
using JSRuntimeExtensions = Blazor.Diagrams.Extensions.JSRuntimeExtensions;

namespace Blazor.Diagrams.Tests.Extensions
{
	public class JSRuntimeExtensionsTest
	{
		[Fact]
		public async Task TestGetBoundingClientRectDoesNotThrowOnTimeout()
		{
			var jsRuntime = new Mock<IJSRuntime>();
			jsRuntime.Setup(j => j.InvokeAsync<Rectangle>(It.IsAny<string>(), It.IsAny<object?[]?>())).ThrowsAsync(new TaskCanceledException());
			var result = await JSRuntimeExtensions.GetBoundingClientRect(jsRuntime.Object, default);
			Assert.Null(result);
		}
	}
}
