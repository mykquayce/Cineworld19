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
        private readonly IEnumerable<Filter> _filters;
        private readonly ISerializationService _serializationService;
        private const RegexOptions _regexOptions = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;

        public FilterService(
            IOptions<FilterCollection> filterCollectionOptions,
            ISerializationService serializationService)
        {
			Guard
				.Argument(() => filterCollectionOptions)
				.NotNull()
				.Require(o => (o.Value?.Count ?? 0) > 0, _ => "No filters were found.");

			Guard
				.Argument(() => serializationService)
				.NotNull();

            _filters = filterCollectionOptions.Value;
			_serializationService = serializationService;
        }

		public cinemasType Filter(cinemasType before)
		{
			var cinemases = new List<List<cinemaType>>();

			foreach (var filter in _filters)
			{
				var cinemas = new List<cinemaType>();

				foreach (var cinema in Filter(before.cinema, filter))
				{
					var films = new List<filmType>();

					foreach (var film in Filter(cinema.listing, filter))
					{
						var shows = Filter(film.shows, filter).ToArray();

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
		public static IEnumerable<cinemaType> Filter(IEnumerable<cinemaType> cinemas, Filter filter)
            => Filter(cinemas, filter.CinemaIds);

		public static IEnumerable<cinemaType> Filter(IEnumerable<cinemaType> cinemas, ICollection<short> cinemaIds)
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
        public static IEnumerable<filmType> Filter(IEnumerable<filmType> films, Filter filter)
            => Filter(films, filter.TitlePatterns);

		public static IEnumerable<filmType> Filter(IEnumerable<filmType> films, ICollection<string> titlePatterns)
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
        public static IEnumerable<showType> Filter(IEnumerable<showType> shows, Filter filter)
            => Filter(shows, filter.TimesOfDay, filter.DaysOfWeek);

		public static IEnumerable<showType> Filter(IEnumerable<showType> shows, TimesOfDay timeFilter, ICollection<DayOfWeek> daysOfWeek)
		{
			Guard
				.Argument(() => shows)
				.NotNull()
				.NotEmpty()
				.Require(a => a.All(i => i != default), _ => nameof(shows) + " has null items");

            Guard
                .Argument(() => daysOfWeek)
				.NotNull()
				.Require(a => a.All(i => i != default), _ => nameof(daysOfWeek) + " has null items");

			foreach (var show in shows)
			{
                if (MatchesTimeOfDay(show.time.TimeOfDay, timeFilter)
                    && (daysOfWeek.Count == 0 || daysOfWeek.Contains(show.time.DayOfWeek)))
                {
                    yield return show;
                }
			}
		}

        private static bool MatchesTimeOfDay(TimeSpan time, TimesOfDay timesOfDay)
        {
            Guard.Argument(() => time).InRange(TimeSpan.FromDays(0), TimeSpan.FromDays(1));

            if (timesOfDay == TimesOfDay.None)
            {
                return true;
            }

            if ((timesOfDay & TimesOfDay.Night) != 0
                && time <= TimeSpan.FromHours(6))
            {
                return true;
            }

            if ((timesOfDay & TimesOfDay.Morning) != 0
                && time >= TimeSpan.FromHours(6)
                && time <= TimeSpan.FromHours(12))
            {
                return true;
            }

            if ((timesOfDay & TimesOfDay.Afternoon) != 0
                && time >= TimeSpan.FromHours(12)
                && time <= TimeSpan.FromHours(18))
            {
                return true;
            }

            if ((timesOfDay & TimesOfDay.Evening) != 0
                && time >= TimeSpan.FromHours(18))
            {
                return true;
            }

            return false;
        }
		#endregion showType filter
	}
}
