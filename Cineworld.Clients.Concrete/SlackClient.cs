﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cineworld.Clients.Concrete
{
	public class SlackClient : ClientBase, ISlackClient
	{
		private readonly Models.Configuration.ApiSettings _apiSettings;

		public SlackClient(
			IHttpClientFactory httpClientFactory,
			IOptions<Models.Configuration.ApiSettings> apiSettingsOptions)
			: base(httpClientFactory)
		{
			_apiSettings = apiSettingsOptions?.Value ?? throw new KeyNotFoundException();
		}

		public async Task<bool> SendMessageAsync(string text)
		{
			var relativeUrl = String.Concat("/services/", string.Join('/', _apiSettings.SlackWebhook));

			var result = await base.PostAsync<string>(relativeUrl, new { text, }).ConfigureAwait(false);

			return string.Equals("ok", result, StringComparison.InvariantCulture);
		}
	}
}
