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
        private readonly FilterCollection _filters;
        private readonly ISerializationService _serializationService;
        private const RegexOptions _regexOptions = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;

        public FilterService(
            IOptions<FilterCollection> filtersOptions,
            ISerializationService serializationService)
        {
            Guard.Argument(() => filtersOptions)
                .NotNull()
                .Require(o => o.Value != default, _ => nameof(filtersOptions) + " cannot be null")
                .Require(o => o.Value.Count > 0, _ => nameof(filtersOptions) + " cannot be empty")
                .Require(o => o.Value.All(f => f.CinemaIds != default), _ => nameof(Models.Configuration.Filter.CinemaIds) + " cannot be null")
                .Require(o => o.Value.All(f => f.DaysOfWeek != default), _ => nameof(Models.Configuration.Filter.CinemaIds) + " cannot be null");

            Guard.Argument(() => serializationService)
                .NotNull();

            _filters = filtersOptions.Value;
            _serializationService = serializationService;
        }

        public cinemasType Filter(cinemasType before)
        {
			Guard
				.Argument(() => before)
				.NotNull()
				.Require(c => (c.cinema?.Length ?? 0) > 0, _ => nameof(before) + " has no cinemas")
				.Require(c => c.cinema.All(i => i != default), _ => nameof(before) + " has null cinemas");

            foreach (var filter in _filters)
            {
                var cinemas = new List<cinemaType>();

                foreach (var cinema in FilterCinemas(before.cinema, filter.CinemaIds))
                {
                    var films = new List<filmType>();

                    foreach (var film in FilterFilms(cinema.listing, filter.TitlePatterns))
                    {
                        var shows = FilterShows(film.shows, filter.TimeFilters, filter.DaysOfWeek).ToList();

                        if (shows.Count > 0)
                        {
                            var cloned = _serializationService.DeepClone(film);

                            cloned.shows = shows.ToArray();

                            films.Add(cloned);
                        }
                    }

                    if (films.Count > 0)
                    {
                        var cloned = _serializationService.DeepClone(cinema);

                        cloned.listing = films.ToArray();

                        cinemas.Add(cloned);
                    }
                }

                return new cinemasType { cinema = cinemas.ToArray(), };
            }

            // TODO: merge cinemaType's into one

            throw new NotImplementedException();
        }

        public static IEnumerable<cinemaType> FilterCinemas(IEnumerable<cinemaType> cinemas, ICollection<short> cinemaIds)
        {
            Guard.Argument(() => cinemas).NotNull().NotEmpty();
            Guard.Argument(() => cinemaIds).NotNull().Require(a => a.All(i => i > 0), _ => nameof(cinemaIds) + " must all be greater than zero");

            return cinemaIds.Count > 0
                ? cinemas.Where(c => cinemaIds.Contains(c.id))
                : cinemas;
        }

        public static IEnumerable<filmType> FilterFilms(IEnumerable<filmType> films, ICollection<string> titlePatterns)
        {
            Guard.Argument(() => films).NotNull().NotEmpty();
            Guard.Argument(() => titlePatterns).NotNull().Require(a => !a.Any(string.IsNullOrWhiteSpace), _ => nameof(titlePatterns) + " must all have content");

            if (titlePatterns.Count == 0)
            {
                foreach (var film in films)
                {
                    yield return film;
                }

                yield break;
            }

            var regexes = titlePatterns.Select(p => new Regex(p, _regexOptions)).ToList();

            foreach (var film in films)
            {
                if (regexes.Any(r => r.Match(film.title).Success))
                {
                    yield return film;
                }
            }
        }

        public static IEnumerable<showType> FilterShows(IEnumerable<showType> shows, ICollection<string> timeFilters, ICollection<DayOfWeek> daysOfWeek)
            => FilterShows(shows, timeFilters.Select(s => new TimeFilter(s)).ToList(), daysOfWeek);

        public static IEnumerable<showType> FilterShows(IEnumerable<showType> shows, ICollection<TimeFilter> timeFilters, ICollection<DayOfWeek> daysOfWeek)
        {
            Guard.Argument(() => shows).NotNull().NotEmpty();

            if (timeFilters.Count == 0 && daysOfWeek.Count == 0)
            {
                foreach (var show in shows)
                {
                    yield return show;
                }

                yield break;
            }

            foreach (var show in shows)
            {
                if ((daysOfWeek.Count == 0 || daysOfWeek.Contains(show.time.DayOfWeek))
                    && (timeFilters.Count == 0 || timeFilters.All(f => f.Func(show.time.TimeOfDay))))
                {
                    yield return show;
                }
            }
        }
    }
}
