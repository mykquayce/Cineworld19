using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class DoNothing : StepBody
	{
		public override ExecutionResult Run(IStepExecutionContext context)
			=> ExecutionResult.Next();
	}
}
