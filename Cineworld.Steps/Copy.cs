using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class Copy : StepBody
	{
		public object Value { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
			=> ExecutionResult.Next();
	}
}
