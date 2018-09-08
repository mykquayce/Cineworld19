using Cineworld.Models;
using Cineworld.Models.Configuration;
using Dawn;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cineworld.Services.Concrete
{
	public class FilterService : IFilterService
    {
        private readonly ICollection<FilterCollection> _filterCollections;
        private readonly ISerializationService _serializationService;
        private const RegexOptions _regexOptions = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;

        public FilterService(
            IOptions<FilterCollectionCollection> filterCollectionCollectionOptions,
            ISerializationService serializationService)
        {
            _filterCollections = filterCollectionCollectionOptions?.Value ?? throw new ArgumentNullException(nameof(filterCollectionCollectionOptions));
			_serializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
        }

		private IEnumerable<int> FilterCinemaIds => from f in _filters
													where (f?.FilterType & FilterTypes.CinemaId) != 0
													let s = f.Value as string
													where !string.IsNullOrWhiteSpace(s)
													let i = int.TryParse(s, out var result) ? result : -1
													where i > 0
													select i;

		private IEnumerable<string> FilterTitles => from f in _filters
													where (f?.FilterType & FilterTypes.Title) != 0
													let s = f.Value as string
													where !string.IsNullOrWhiteSpace(s)
													select s;

		private IEnumerable<DayOfWeek> FilterDaysOfWeek => from f in _filters
														   where (f?.FilterType & FilterTypes.DayOfWeek) != 0
														   let s = f.Value as string
														   where !string.IsNullOrWhiteSpace(s)
														   let d = Enum.TryParse(s, out DayOfWeek result) ? result : (DayOfWeek)(-1)
														   where d >= 0
														   select d;

        public cinemasType Filter(cinemasType before)
		{
			var cinemas = new List<cinemaType>();

			var cloned = _serializationService.DeepClone(before.cinema);

			foreach (var cinema in FilterCinemas(cloned))
			{
				var films = new List<filmType>();

				foreach (var film in FilterFilms(cinema.listing))
				{
					var shows = FilterShows(film.shows).ToArray();

					if (shows.Length > 0)
					{
						film.shows = shows;
						films.Add(film);
					}
				}

				if (films.Count > 0)
				{
					cinema.listing = films.ToArray();
					cinemas.Add(cinema);
				}
			}

			return new cinemasType { cinema = cinemas.ToArray(), };
		}

		private IEnumerable<cinemaType> FilterCinemas(IEnumerable<cinemaType> cinemas)
		{
			Guard.Argument(() => cinemas)
				.NotNull()
				.Require(cc => cc.All(c => c != default), _ => $"{nameof(cinemas)} cannot have null items");

			var ids = FilterCinemaIds?.ToList() ?? new List<int>();

			if (ids.Count == 0)
			{
				return cinemas;
			}

			return cinemas.Where(c => ids.Contains(c.id));
		}

		private IEnumerable<filmType> FilterFilms(IEnumerable<filmType> films)
		{
			Guard.Argument(() => films)
				.NotNull()
				.Require(ff => ff.All(c => c != default), _ => $"{nameof(films)} cannot have null items");

			var titles = FilterTitles?.ToList() ?? new List<string>();

			if (titles.Count == 0)
			{
				return films;
			}

			return from f in films
				   where titles.Any(t => f.title.IndexOf(t, StringComparison.InvariantCultureIgnoreCase) > -1)
				   select f;
		}

		private IEnumerable<showType> FilterShows(IEnumerable<showType> shows)
		{
			Guard.Argument(() => shows)
				.NotNull()
				.Require(ss => ss.All(s => s != default), _ => $"{nameof(shows)} cannot have null items");

			var days = FilterDaysOfWeek?.ToList() ?? new List<DayOfWeek>();

			if (days.Count == 0)
			{
				return shows;
			}

			return shows.Where(s => days.Contains(s.time.DayOfWeek));
		}
    }
}
