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

            throw new NotImplementedException();
        }

        public static IEnumerable<showType> MergeShows(params IEnumerable<showType>[] showses)
            => Merge(showses).OrderBy(s => s.time);

        public static IEnumerable<T> Merge<T>(params IEnumerable<T>[] collections)
            => collections.SelectMany(c => c).GroupBy(i => i).Select(g => g.Key);
    }
}
