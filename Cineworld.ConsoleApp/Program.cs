using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using WorkflowCore.Interface;

namespace Cineworld.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Environment.CurrentDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			IConfiguration configuration = builder.Build();

			var services = new ServiceCollection();

			void AddHttpClient(string name, Func<Models.Configuration.ApiSettings, string> getUrl)
			{
				services
					.AddHttpClient(name)
					.ConfigureHttpClient((provider, client) =>
					{
						var apiSettingsOptions = provider.GetRequiredService<IOptions<Models.Configuration.ApiSettings>>();
						var apiSettings = apiSettingsOptions.Value;
						var uri = new Uri(getUrl(apiSettings), UriKind.Absolute);

						client.BaseAddress = uri;
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					});
			}

			AddHttpClient(nameof(Clients.Concrete.CineworldClient), s => s.CineworldUrl);
			AddHttpClient(nameof(Clients.Concrete.SlackClient), s => s.SlackUrl);

			services
				.AddLogging(b => b
					.AddDebug()
					.SetMinimumLevel(LogLevel.Trace)
				)
				.AddWorkflow();

			services
				.Configure<Models.Configuration.Settings>(configuration)
				.Configure<Models.Configuration.ApiSettings>(configuration.GetSection(nameof(Models.Configuration.ApiSettings)))
				.Configure<Models.Configuration.FilterCollectionCollection>(configuration.GetSection(nameof(Models.Configuration.FilterCollectionCollection)));

			services
				.AddTransient<Clients.ICineworldClient, Clients.Concrete.CineworldClient>()
				.AddTransient<Clients.ISlackClient, Clients.Concrete.SlackClient>();

			services
				.AddTransient<Services.IFileSystemService, Services.Concrete.FileSystemService>()
				.AddTransient<Services.IFilterService, Services.Concrete.FilterService>()
				.AddTransient<Services.ISerializationService, Services.Concrete.SerializationService>();

			services
				.AddTransient<Steps.FilterData>()
				.AddTransient<Steps.GetLatestShowingsFromLocal>()
				.AddTransient<Steps.GetLatestShowingsFromRemote>()
				.AddTransient<Steps.GetShowingsLastModifiedFromLocal>()
				.AddTransient<Steps.GetShowingsLastModifiedFromRemote>()
				.AddTransient<Steps.Save>()
				.AddTransient<Steps.SendMessage>()
				.AddTransient<Steps.Sleep>();

			var serviceProvider = services.BuildServiceProvider();

			var loggerFactory = serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
			loggerFactory.AddDebug();

			var host = serviceProvider.GetService<IWorkflowHost>();

			host.RegisterWorkflow<Workflows.MoviesCheck, Models.PersistanceData>();

			host.OnStepError += (workflow, step, exception) =>
			{
				var s = string.Join(Environment.NewLine, Models.Helpers.ExtensionMethods.ToStrings(exception));

				Console.WriteLine($"{DateTime.UtcNow:O} {step.Name} {s}");
			};

			host.Start();

			host.StartWorkflow(nameof(Workflows.MoviesCheck), 1, new Models.PersistanceData());

			Thread.Sleep(int.MaxValue);

			host.Stop();
		}
	}
}
