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
		private readonly Services.ISerializationService _serializationService;

		public FilterData(
			Services.ISerializationService serializationService)
		{
			_serializationService = serializationService;
		}

		public cinemasType Original { get; set; }
		public cinemasType Filtered { get; set; }

        private static IEnumerable<T> Filter<T>(IEnumerable<T> collection, IEnumerable<Predicate<T>> predicates)
        {
            if (!(predicates?.Any() ?? false))
            {
                return collection;
            }

            return from item in collection
                   where (from p in predicates
                          where p(item)
                          select 1).Any()
                   select item;
        }

        private static IEnumerable<T> Filter<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            return predicate == null
                ? collection
                : collection.Where(item => predicate(item));
        }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
            throw new NotImplementedException();
		}
	}
}
