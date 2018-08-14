using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Cineworld.Clients.Tests
{
	public class CineworldClientTests : IDisposable
	{
		private readonly Clients.ICineworldClient _client;
		private readonly Stack<IDisposable> _disposables = new Stack<IDisposable>();

		public CineworldClientTests()
		{
			var httpClient = new HttpClient { BaseAddress = new Uri("https://www.cineworld.co.uk/", UriKind.Absolute), };
			var httpClientFactoryMock = new Moq.Mock<IHttpClientFactory>();

			httpClientFactoryMock
				.Setup(x => x.CreateClient(Moq.It.IsAny<string>()))
				.Returns(httpClient);

			_client = new Clients.Concrete.CineworldClient(httpClientFactoryMock.Object);

			_disposables.Push(httpClient);
		}

		public void Dispose()
		{
			while (_disposables.Any())
			{
				_disposables.Pop()?.Dispose();
			}
		}

		[Fact]
		public void CineworldClientTests_GetLastModifiedAsync_ReturnsLastModifiedInLessThanFiveSeconds()
		{
			// Arrange
			var stopwatch = Stopwatch.StartNew();

			// Act
			var lastModified = _client.GetListingsUkLastModifiedAsync()
				.ConfigureAwait(false).GetAwaiter().GetResult();

			// Arrange
			stopwatch.Stop();

			// Assert
			Assert.NotEqual(default(DateTime), lastModified);
			Assert.Equal(DateTimeKind.Utc, lastModified.Kind);
			Assert.InRange(lastModified, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow);
			Assert.InRange(stopwatch.ElapsedMilliseconds, 1, 5_000);
		}
    }
}
