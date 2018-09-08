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
					.Then<PrintMessage>()
						.Input(step => step.Message, data => $"LastModifiedFromRemote: {data.LastModifiedFromRemote:O}")
						.Input(step => step.Message, data => $"LastModifiedFromLocal: {data.LastModifiedFromLocal:O}")
					.Then<GetShowingsLastModifiedFromRemote>()
						.Output(data => data.LastModifiedFromRemote, step => step.LastModified)
					.If(data => (data.LastModifiedFromLocal ?? DateTime.MinValue) < data.LastModifiedFromRemote)
					.Do(then2 => then2
						.StartWith<GetLatestShowingsFromRemote>()
							.Output(data => data.ShowingsFromRemote, step => step.Cinemas)
						.Then<PrintMessage>()
							.Input(step => step.Message, data => "ShowingsFromRemote: " + CinemasToString(data.ShowingsFromRemote))
						.Then<FilterData>()
							.Input(step => step.Original, data => data.ShowingsFromRemote)
							.Output(data => data.FilteredShowingsFromRemote, step => step.Filtered)
						.Then<PrintMessage>()
							.Input(step => step.Message, data => "FilteredShowingsFromRemote: " + CinemasToString(data.FilteredShowingsFromRemote))
						.Then<CompareData>()
							.Input(step => step.Local, data => data.FilteredShowingsFromLocal)
							.Input(step => step.Remote, data => data.FilteredShowingsFromLocal)
							.Output(data => data.HaveNewData, step => step.AreIdentical == false)
						.Then<PrintMessage>()
							.Input(step => step.Message, data => $"HaveNewData: {data.HaveNewData}")
						.If(data => data.HaveNewData ?? false)
							.Do(then4 => then4
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

		private static string CinemasToString(cinemasType cinemas) => string.Join("; ", CinemasToStrings(cinemas));

		private static System.Collections.Generic.IEnumerable<string> CinemasToStrings(cinemasType cinemas)
		{
			try
			{
				yield return (cinemas?.cinema?.Length ?? 0) + " cinema(s)";

				var ashton = new cinemaType { listing = cinemas?.cinema?.Where(c => c.id == 23)?.SelectMany(c => c?.listing)?.ToArray() ?? new filmType[0], };

				yield return ashton?.listing?.Length + " film(s) at Ashton";

				var today = DateTime.Now.Date;
				var friday = Enumerable.Range(0, 7).Select(i => today.AddDays(i)).Single(dt => dt.DayOfWeek == DayOfWeek.Friday);

				var films = ashton?.listing?.Where(f => f.shows.Any(s => s.time.Date == friday))?.ToList();

				yield return films?.Count + " film(s) on Friday";
			}
			finally { }
		}
	}

	public class PrintMessage : StepBody
	{
		public string Message { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			Console.WriteLine("{0:O} : {1}", DateTime.UtcNow, Message);

			return ExecutionResult.Next();
		}
	}
}
