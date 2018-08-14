using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class Sleep : StepBody
	{
		public TimeSpan Duration { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			return context.PersistenceData is default(object)
				? ExecutionResult.Sleep(Duration, new object())
				: ExecutionResult.Next();
		}
	}
}
