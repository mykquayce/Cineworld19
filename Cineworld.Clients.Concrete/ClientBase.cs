using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cineworld.Clients.Concrete
{
	public abstract class ClientBase
	{
		private readonly string _name;
		private readonly IHttpClientFactory _httpClientFactory;

		protected ClientBase(
			IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory
				?? throw new ArgumentNullException(nameof(httpClientFactory));

			_name = this.GetType().Name;
		}

		protected async Task<DateTime> GetLastModifiedAsync(string url)
		{
			using (var httpResponseMessage = await GetResponseAsync(url).ConfigureAwait(false))
			{
				using (var httpContent = httpResponseMessage.Content)
				{
					return httpContent.Headers.LastModified?.UtcDateTime
						?? throw new InvalidOperationException($"{url} didn't return a {nameof(System.Net.Http.Headers.HttpContentHeaders.LastModified)}");
				}
			}
		}

		protected async Task<Stream> DownloadFileAsync(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentNullException(nameof(url));
			}

			var httpResponseMessage = await GetResponseAsync(url).ConfigureAwait(false);
			var httpContent = httpResponseMessage.Content;
			return await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
		}

		protected async Task<T> PostAsync<T>(string url, object requestBody)
		{
			var httpResponseMessage = await GetResponseAsync(url, requestBody).ConfigureAwait(false);

			using(httpResponseMessage)
			{
				using (var content = httpResponseMessage.Content)
				{
					var responseBody = await content.ReadAsStringAsync().ConfigureAwait(false);

					if (string.IsNullOrWhiteSpace(responseBody))
					{
						return default(T);
					}

					return typeof(T) == typeof(string)
						? (T)Convert.ChangeType(responseBody, typeof(T))
						: JsonConvert.DeserializeObject<T>(responseBody);
				}
			}
		}

		private Task<HttpResponseMessage> GetResponseAsync(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentNullException(nameof(url));
			}

			var client = _httpClientFactory.CreateClient(_name);

			using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url))
			{

				return client.SendAsync(httpRequestMessage);
			}
		}

		private async Task<HttpResponseMessage> GetResponseAsync(string url, object requestBody)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentNullException(nameof(url));
			}

			var requestJson = JsonConvert.SerializeObject(requestBody);

			var client = _httpClientFactory.CreateClient(_name);

			using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url))
			{

				using (var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json"))
				{
					httpRequestMessage.Content = requestContent;

					return await client.SendAsync(httpRequestMessage).ConfigureAwait(false);
				}
			}
		}
	}
}
