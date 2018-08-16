using Cineworld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
	public class FilterData : StepBody
	{
        private readonly Services.IFilterService _filterService;

		public FilterData(
			Services.IFilterService filterService)
		{
            _filterService = filterService;
		}

		public cinemasType Original { get; set; }
		public cinemasType Filtered { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
            Filtered = new cinemasType { cinema = _filterService.Filter(Original.cinema), };

			return ExecutionResult.Next();
		}
	}
}
