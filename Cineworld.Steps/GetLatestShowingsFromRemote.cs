using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class GetLatestShowingsFromRemote : StepBodyAsync
	{
		private readonly Clients.ICineworldClient _client;
		private readonly Services.ISerializationService _serializationService;

		public GetLatestShowingsFromRemote(
			Clients.ICineworldClient client,
			Services.ISerializationService serializationService)
		{
			_client = client
				?? throw new ArgumentNullException(nameof(Clients.ICineworldClient));

			_serializationService = serializationService
				?? throw new ArgumentNullException(nameof(Services.ISerializationService));
		}

		public Models.cinemasType Cinemas { get; set; }

		public async override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
		{
			using (var stream = await _client.DownloadListingsUkAsync().ConfigureAwait(false))
			{
				Cinemas = _serializationService.Deserialize<Models.cinemasType>(stream);
			}

			return ExecutionResult.Next();
		}
	}
}
