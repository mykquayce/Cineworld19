using Cineworld.Models;
using Cineworld.Models.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cineworld.Services.Concrete
{
    public class FilterService : IFilterService
    {
        private const RegexOptions _regexOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;

        private readonly ICollection<Filter> _filters;
        private readonly ISerializationService _serializationService;

        public FilterService(
            IOptions<List<Filter>> filterOptions,
            ISerializationService serializationService)
        {
            _filters = filterOptions.Value;
            _serializationService = serializationService;
        }

        public IEnumerable<cinemasType> Filter(cinemasType before)
        {
            foreach (var filter in _filters)
            {
                var cinemas = new List<cinemaType>();

                foreach (var cinema in Filter(before.cinema, filter))
                {
                    var films = new List<filmType>();

                    foreach (var film in Filter(cinema.listing, filter))
                    {
                        var shows = _serializationService.DeepClone(Filter(film.shows, filter).ToArray());

                        var cloned = _serializationService.DeepClone(film);

                        cloned.shows = shows;

                        films.Add(cloned);
                    }

                    var clonedCinema = _serializationService.DeepClone(cinema);

                    clonedCinema.listing = films.ToArray();

                    cinemas.Add(clonedCinema);
                }

                yield return new cinemasType { cinema = cinemas.ToArray(), };
            }
        }


        private static IEnumerable<cinemaType> Filter(IEnumerable<cinemaType> cinemas, Filter filter)
        {
            if (filter?.CinemaIds?.Any() ?? false)
            {
                return from c in cinemas
                       where filter.CinemaIds.Contains(c.id)
                       select c;
            }

            return cinemas;
        }

        private static IEnumerable<filmType> Filter(IEnumerable<filmType> films, Filter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter?.TitlePattern))
            {
                var titleRegex = string.IsNullOrWhiteSpace(filter.TitlePattern)
                    ? default(Regex)
                    : new Regex(filter.TitlePattern, _regexOptions);

                return from f in films
                       where titleRegex.IsMatch(f.title)
                       select f;
            }

            return films;
        }

        private static IEnumerable<showType> Filter(IEnumerable<showType> shows, Filter filter)
        {
            if (filter?.DaysOfWeek?.Any() ?? false)
            {
                return from s in shows
                       where filter.DaysOfWeek.Contains(s.time.DayOfWeek)
                       select s;
            }

            return shows;
        }
    }
}
