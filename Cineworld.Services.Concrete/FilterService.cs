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
        private const RegexOptions _regexOptions = RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;

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

        public cinemasType Filter(cinemasType cinemas)
        {
            foreach (var filter in _filters)
            {
                foreach (var cinema in FilterCinemas(cinemas.cinema, filter.CinemaIds))
                {

                }
            }

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

        public static IEnumerable<filmType> Filter(IEnumerable<filmType> films, ICollection<string> titlePatterns)
        {
            Guard.Argument(() => films).NotNull().NotEmpty();
            Guard.Argument(() => titlePatterns).NotNull().Require(a => !a.Any(string.IsNullOrWhiteSpace), _ => nameof(titlePatterns) + " must all have content");

            if (titlePatterns.Count == 0)
            {
                foreach (var film in films)
                {
                    yield return film;
                }
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
    }
}
