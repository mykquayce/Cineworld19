using System;
using System.Collections.Generic;
using System.Linq;

namespace Cineworld.Models.Helpers
{
	public static class ExtensionMethods
	{
		public static string ToRelativeUrlString(this CineworldLists lists)
		{
			switch (lists)
			{
				case CineworldLists.AllPerformancesEire:
					return "/syndication/all-performances_ie.xml";
				case CineworldLists.AllPerformancesUk:
					return "/syndication/all-performances.xml";

				case CineworldLists.CinemasEire:
					return "/syndication/cinemas_ie.xml";
				case CineworldLists.CinemasUk:
					return "/syndication/cinemas.xml";

				case CineworldLists.FilmTimesEire:
					return "/syndication/film_times_ie.xml";
				case CineworldLists.FilmTimesUk:
					return "/syndication/film_times.xml";

				case CineworldLists.ListingsEire:
					return "/syndication/listings_ie.xml";
				case CineworldLists.ListingsUk:
					return "/syndication/listings.xml";

				case CineworldLists.WeeklyFilmTimesEire:
					return "/syndication/weekly_film_times_ie.xml";
				case CineworldLists.WeeklyFilmTimesUk:
					return "/syndication/weekly_film_times.xml";

				default:
					throw new ArgumentOutOfRangeException(nameof(lists), lists, $"Unexpected {nameof(lists)}")
					{
						Data = { { nameof(lists), lists }, },
					};
			}
		}

		public static bool CollectionEquals<T>(this IEnumerable<T> left, IEnumerable<T> right) where T : IEquatable<T>
			=> left?.All(l => right?.Any(r => l?.Equals(r) ?? false) ?? false) ?? false;

		public static bool SafeEquals(this string left, string right)
			=> (left is default(string) && right is default(string))
			|| (left?.Equals(right, StringComparison.InvariantCulture) ?? false);

		public static IEnumerable<string> ToStrings(this Exception exception, string indent = "")
		{
			yield return indent + DateTime.UtcNow.ToString("O");
			yield return indent + exception.GetType().FullName;
			yield return indent + exception.Message;
			yield return indent + exception.StackTrace;

			foreach (var key in exception.Data.Keys)
			{
				var value = exception.Data[key];

				yield return $"{indent}{key}={value};";
			}

			yield return new string('=', 10);

			switch (exception)
			{
				case AggregateException aggregateException:
					foreach(var innerException in aggregateException.InnerExceptions)
					{
						foreach (var s in innerException.ToStrings(indent + '\t'))
						{
							yield return indent + s;
						}
					}
					break;
				default:
					if (!(exception.InnerException is default(Exception)))
					{
						foreach (var s in exception.InnerException.ToStrings(indent + '\t'))
						{
							yield return indent + s;
						}
					}
					break;
			}
		}
	}
}
