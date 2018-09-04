using Cineworld.Models;
using Cineworld.Steps;
using System;
using System.Diagnostics;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Workflows
{
	public class MoviesCheck : IWorkflow<PersistanceData>
	{
		public string Id => GetType().Name;
		public int Version => 1;

		public void Build(IWorkflowBuilder<PersistanceData> builder)
		{
			var today = DateTime.UtcNow.Date;

			var nearestFriday = Enumerable.Range(0, 7)
				.Select(i => today.AddDays(i))
				.Single(d => d.DayOfWeek == DayOfWeek.Friday);

			builder
				.StartWith<DoNothing>()
				.Parallel()
				.Do(then => then
					.StartWith<GetShowingsLastModifiedFromLocal>()
						.Output(data => data.LastModifiedFromLocal, step => step.LastModified)
					)
				.Do(then => then
					.StartWith<GetLatestShowingsFromLocal>()
						.Output(data => data.FilteredShowingsFromLocal, step => step.Cinemas)
					)
				.Join()
				.While(_ => true)
				.Do(then => then
					.StartWith<PrintMessage>()
						.Input(step => step.Message, _ => "New lap")
					.Then<DoNothing>()
						.Output(data => data.LastModifiedFromRemote, _ => default(DateTime))
					.Then<GetShowingsLastModifiedFromRemote>()
						.Output(data => data.LastModifiedFromRemote, step => step.LastModified)
					.If(data => (data.LastModifiedFromLocal ?? DateTime.MinValue) < data.LastModifiedFromRemote)
					.Do(then2 => then2
						.StartWith<GetLatestShowingsFromRemote>()
							.Output(data => data.ShowingsFromRemote, step => step.Cinemas)
						.Then<FilterData>()
							.Input(step => step.Original, data => data.ShowingsFromRemote)
							.Output(data => data.FilteredShowingsFromRemote, step => step.Filtered)
						.Then<CompareData>()
							.Input(step => step.Local, data => data.FilteredShowingsFromLocal)
							.Input(step => step.Remote, data => data.FilteredShowingsFromRemote)
							.Output(data => data.HaveNewData, step => step.AreIdentical == false)
						.If(data => data.HaveNewData ?? false)
							.Do(then3 => then3
								.StartWith<Copy>()
									.Input(step => step.Value, data => data.FilteredShowingsFromRemote)
									.Output(data => data.FilteredShowingsFromLocal, step => step.Value)
								.Then<Copy>()
									.Input(step => step.Value, data => data.LastModifiedFromRemote)
									.Output(data => data.LastModifiedFromLocal, step => step.Value)
								.Then<DoNothing>()
									.Output(data => data.LastModifiedFromRemote, _ => DateTime.MinValue)
									.Output(data => data.FilteredShowingsFromRemote, _ => default(cinemaType))
									.Output(data => data.ShowingsFromRemote, _ => default(cinemaType))
								.Then<Save>()
									.Input(step => step.Cinemas, data => data.FilteredShowingsFromLocal)
									.Input(step => step.LastModified, data => data.LastModifiedFromLocal.Value)
								.Then<SendMessage>()
									.Input(step => step.Message, data => data.FilteredShowingsFromLocal)
							)
					)
					// Sleeeeeeeeeep...
					.Then<Sleep>()
						.Input(step => step.Duration, _ => TimeSpan.FromMinutes(30))
				);
		}
	}

	public class PrintMessage : StepBody
	{
		public string Message { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			Console.WriteLine($"{DateTime.UtcNow:O} : {{0}}", Message);

			return ExecutionResult.Next();
		}
	}
}
