using Dawn;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cineworld.Models.Helpers
{
    public static class MergeHelpers
    {
        public static IEnumerable<cinemaType> MergeCinemas(params IEnumerable<cinemaType>[] cinemases)
        {
            Guard.Argument(() => cinemases).NotNull().NotEmpty().CountInRange(2, int.MaxValue);

			foreach (var (id, group) in from cc in cinemases
										from c in cc
										group c by c.id into gg
										select (gg.Key, gg))
			{
				var cinema = new cinemaType
				{
					id = id,
					listing = MergeFilms(group.SelectMany(g => g.listing)).ToArray(),
                    name = group.First().name,
				};

				yield return cinema;
			}
        }

		public static IEnumerable<filmType> MergeFilms(params IEnumerable<filmType>[] filmses)
		{
			var merged = new List<filmType>();

			foreach (var (edi, group) in from ff in filmses
										  from f in ff
										  group f by f.edi into gg
										  select (gg.Key, gg))
			{
				var film = new filmType
				{
					edi = edi,
					title = group.First().title,
					shows = group.SelectMany(f => f.shows).Distinct().ToArray(),
				};

				merged.Add(film);
			}

			return merged;
		}

        public static IEnumerable<showType> MergeShows(params IEnumerable<showType>[] showses)
            => Merge(showses).OrderBy(s => s.time);

        public static IEnumerable<T> Merge<T>(params IEnumerable<T>[] collections)
            => collections.SelectMany(c => c).GroupBy(i => i).Select(g => g.Key);
    }
}
