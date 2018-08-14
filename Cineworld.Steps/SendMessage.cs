using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class SendMessage : StepBodyAsync
	{
		private readonly Clients.ISlackClient _slackClient;

		public SendMessage(Clients.ISlackClient slackClient)
		{
			_slackClient = slackClient;
		}

		public object Message { get; set; }

		public async override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
		{
			var s = Message?.ToString();

			if (string.IsNullOrWhiteSpace(s))
			{
				return ExecutionResult.Next();
			}

			await _slackClient.SendMessageAsync("<!channel> " + s).ConfigureAwait(false);

			return ExecutionResult.Next();
		}
	}
}
