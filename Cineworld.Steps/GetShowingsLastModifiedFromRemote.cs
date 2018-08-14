using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class GetShowingsLastModifiedFromRemote : StepBodyAsync
	{
		private readonly Clients.ICineworldClient _client;

		public GetShowingsLastModifiedFromRemote(
			Clients.ICineworldClient client)
		{
			_client = client
				?? throw new ArgumentNullException(nameof(Clients.ICineworldClient));
		}

		public DateTime LastModified { get; set; }

		public async override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
		{
			LastModified = await _client.GetListingsUkLastModifiedAsync().ConfigureAwait(false);

			return ExecutionResult.Next();
		}
	}
}
