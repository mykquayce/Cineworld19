using Cineworld.Models;
using Cineworld.Models.Configuration;
using Dawn;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cineworld.Services.Concrete
{
	public class FilterService : IFilterService
    {
        private readonly IEnumerable<IEnumerable<Filter>> _filterses;
        private readonly ISerializationService _serializationService;
        private const RegexOptions _regexOptions = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;

        public FilterService(
            IOptions<FilterCollectionCollection> filterCollectionCollectionOptions,
            ISerializationService serializationService)
        {
			Guard
				.Argument(() => filterCollectionCollectionOptions)
				.NotNull()
				.Require(o => (o.Value?.Count ?? 0) > 0, _ => "No filters were found.");

			Guard
				.Argument(() => serializationService)
				.NotNull();

            _filterses = filterCollectionCollectionOptions.Value;
			_serializationService = serializationService;
        }

		public cinemasType Filter(cinemasType before)
		{
			var cinemases = new List<List<cinemaType>>();

			foreach (var filters in _filterses)
			{
				var cinemas = new List<cinemaType>();

				foreach (var cinema in Filter(before.cinema, filters))
				{
					var films = new List<filmType>();

					foreach (var film in Filter(cinema.listing, filters))
					{
						var shows = Filter(film.shows, filters).ToArray();

						if (shows.Length == 0)
						{
							continue;
						}

						var f = _serializationService.DeepClone(film);

						f.shows = shows;

						films.Add(f);
					}

					if (films.Count == 0)
					{
						continue;
					}

					var c = _serializationService.DeepClone(cinema);

					c.listing = films.ToArray();

					cinemas.Add(c);
				}

				if (cinemas.Count == 0)
				{
					continue;
				}

				cinemases.Add(cinemas);
			}

			var merged = Models.Helpers.MergeHelpers.MergeCinemas(cinemases.ToArray());

			return new cinemasType { cinema = merged.ToArray(), };
		}

		#region cinemaType filter
		public static IEnumerable<cinemaType> Filter(IEnumerable<cinemaType> cinemas, IEnumerable<Filter> filters)
		{
			var cinemaIds = from f in filters
							where (f.FilterType & FilterTypes.CinemaId) != FilterTypes.None
							from s in ((string)f.Value).Split(' ', ',', ';')
							let i = short.TryParse(s, out var result) ? result : default
							where i != default
							select i;

			return Filter(cinemas, cinemaIds.ToList());
		}

		public static IEnumerable<cinemaType> Filter(IEnumerable<cinemaType> cinemas, IReadOnlyCollection<short> cinemaIds)
		{
			Guard
				.Argument(() => cinemas)
				.NotNull()
				.NotEmpty()
				.Require(a => a.All(i => i != default), _ => nameof(cinemas) + " has null items");

			Guard
				.Argument(() => cinemaIds)
				.NotNull()
				.Require(a => a.All(i => i > 0), _ => nameof(cinemaIds) + " has null items")
				.Require(a => a.Count == 0 || a.GroupBy(i => i).All(i => i.Count() == 1), _ => nameof(cinemaIds) + " must be unique");

			if (cinemaIds.Count == 0)
			{
				return cinemas;
			}

			return from c in cinemas
				   join id in cinemaIds on c.id equals id
				   select c;
		}
		#endregion cinemaType filter

		#region filmType filter
		public static IEnumerable<filmType> Filter(IEnumerable<filmType> films, IEnumerable<Filter> filters)
		{
			var titlePatterns = from f in filters
								where (f.FilterType & FilterTypes.Title) != FilterTypes.None
								select (string)f.Value;

			return Filter(films, titlePatterns.ToList());
		}

		public static IEnumerable<filmType> Filter(IEnumerable<filmType> films, IReadOnlyCollection<string> titlePatterns)
		{
			Guard
				.Argument(() => films)
				.NotNull()
				.NotEmpty()
				.Require(a => a.All(i => i != default), _ => nameof(films) + " has null items");

			Guard
				.Argument(() => titlePatterns)
				.NotNull()
				.Require(a => !a.Any(string.IsNullOrWhiteSpace), _ => nameof(titlePatterns) + " has null items");

			if (titlePatterns.Count == 0)
			{
				foreach (var film in films)
				{
					yield return film;
				}
			}

			foreach (var film in films)
			{
				if (titlePatterns.Any(p => Regex.IsMatch(film.title, p, _regexOptions)))
				{
					yield return film;
				}
			}
		}
		#endregion filmType filter

		#region showType filter
		public static IEnumerable<showType> Filter(IEnumerable<showType> shows, IEnumerable<Filter> filters)
		{
			var timeFilters = from f in filters
							  where (f.FilterType & FilterTypes.Time) != 0
							  select new TimeFilter((string)f.Value);

			var daysOfWeek = from f in filters
							 where (f.FilterType & FilterTypes.DayOfWeek) != 0
							 let d = Enum.TryParse<DayOfWeek>((string)f.Value, out var result) ? result : default
							 where d != default
							 select d;

			return Filter(shows, timeFilters.ToList(), daysOfWeek.ToList());
		}

		public static IEnumerable<showType> Filter(IEnumerable<showType> shows, IReadOnlyCollection<TimeFilter> timeFilters, IReadOnlyCollection<DayOfWeek> daysOfWeek)
		{
			Guard
				.Argument(() => shows)
				.NotNull()
				.NotEmpty()
				.Require(a => a.All(i => i != default), _ => nameof(shows) + " has null items");

			Guard
				.Argument(() => timeFilters)
				.NotNull()
				.Require(a => a.All(i => i != default), _ => nameof(timeFilters) + " has null items");

			Guard
				.Argument(() => daysOfWeek)
				.NotNull();

			if (timeFilters.Count == 0
				&& daysOfWeek.Count == 0)
			{
				foreach (var show in shows)
				{
					yield return show;
				}

				yield break;
			}

			foreach (var show in shows)
			{
				if ((timeFilters.Count == 0 || timeFilters.Any(f => f.Func(show.time.TimeOfDay)))
					&& (daysOfWeek.Count == 0 || daysOfWeek.Any(d => show.time.DayOfWeek == d)))
				{
					yield return show;
				}
			}
		}
		#endregion showType filter
	}
}
