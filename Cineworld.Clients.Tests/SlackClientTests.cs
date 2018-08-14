using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Cineworld.Clients.Tests
{
	public class SlackClientTests : IDisposable
	{
		private readonly Clients.ISlackClient _client;
		private readonly Stack<IDisposable> _disposables = new Stack<IDisposable>();

		public SlackClientTests()
		{
			var httpClient = new HttpClient { BaseAddress = new Uri("https://hooks.slack.com/", UriKind.Absolute) };

			httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var httpClientFactoryMock = new Moq.Mock<IHttpClientFactory>();

			httpClientFactoryMock
				.Setup(x => x.CreateClient(Moq.It.IsAny<string>()))
				.Returns(httpClient);

			var apiSettings = new Models.Configuration.ApiSettings
			{
				SlackWebhook = { "T4ZU0H1PV", "BC6T5P35F", "vY78BiOYYk6GO0aAbycUJSt5", },
			};

			var options = Moq.Mock.Of<IOptions<Models.Configuration.ApiSettings>>(o => o.Value == apiSettings );

			_client = new Clients.Concrete.SlackClient(httpClientFactoryMock.Object, options);

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
		public void SlackClientTests_SendMessageAsync_ReturnsTrue()
		{
			// Act
			var success = _client.SendMessageAsync("test")
				.ConfigureAwait(false).GetAwaiter().GetResult();

			// Assert
			Assert.True(success);
		}
    }
}
