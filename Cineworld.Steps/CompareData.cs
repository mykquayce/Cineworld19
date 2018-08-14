using Cineworld.Models;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class CompareData : StepBody
	{
		public cinemasType Local { get; set; }
		public cinemasType Remote { get; set; }
		public bool? AreIdentical { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			AreIdentical = Local?.Equals(Remote) ?? false;

			return ExecutionResult.Next();
		}
	}
}
