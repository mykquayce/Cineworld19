using Cineworld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class Output : StepBody
	{
		public cinemasType Cinemas { get; set; }
		public IEnumerable<Action<string>> Outputs { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			var shows = from c in Cinemas.cinema
						from l in c.listing
						from s in l.shows
						orderby c.name, s.time, l.title
						select $@"{c.name} {s.time:yyyy-MM-dd HH\:mm} : {l.title}";

			(
				from s in shows
				from o in Outputs
				select (s, o)
				)
			.ToList()
			.ForEach(tuple => tuple.Item2(tuple.Item1));

			return ExecutionResult.Next();
		}
	}
}
