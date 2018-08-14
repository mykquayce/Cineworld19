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
		public IEnumerable<Predicate<cinemaType>> CinemaFilters { get; set; }
		public IEnumerable<Predicate<filmType>> FilmFilters { get; set; }
		public IEnumerable<Predicate<showType>> ShowFilters { get; set; }

		public override ExecutionResult Run(IStepExecutionContext context)
		{
			var cinemas = new List<cinemaType>();

			foreach (var cinema in FIlterCinemas(Original.cinema))
			{
				var films = new List<filmType>();

				foreach (var film in FilterFilms(cinema.listing))
				{
					var filtered = FIlterShows(film.shows).ToArray();
					var cloned = _serializationService.DeepClone(filtered);

					if (cloned.Any())
					{
						var f = _serializationService.DeepClone(film);

						f.shows = cloned;

						films.Add(f);
					}
				}

				if (films.Any())
				{
					var c = _serializationService.DeepClone(cinema);

					c.listing = films.ToArray();

					cinemas.Add(c);
				}
			}

			Filtered = new cinemasType
			{
				cinema = cinemas.ToArray(),
			};

			return ExecutionResult.Next();
		}

		private IEnumerable<cinemaType> FIlterCinemas(IEnumerable<cinemaType> cinemas) => Filter(cinemas, CinemaFilters);
		private IEnumerable<filmType> FilterFilms(IEnumerable<filmType> films) => Filter(films, FilmFilters);
		private IEnumerable<showType> FIlterShows(IEnumerable<showType> shows) => Filter(shows, ShowFilters);

		private static IEnumerable<T> Filter<T>(IEnumerable<T> items, IEnumerable<Predicate<T>> filters)
		{
			if (filters?.Any() ?? false)
			{
				return from i in items
					   where filters.All(f => f(i))
					   select i;
			}

			return items;
		}
	}
}
