using Cineworld.Models.Helpers;
using Xunit;

namespace Cineworld.Models.Tests.ExtensionMethodsTests
{
	public class CineworldListsToRelativeUrlStringTests
	{
		[Theory]
		[InlineData(CineworldLists.AllPerformancesEire, "/syndication/all-performances_ie.xml")]
		[InlineData(CineworldLists.AllPerformancesUk, "/syndication/all-performances.xml")]
		[InlineData(CineworldLists.CinemasEire, "/syndication/cinemas_ie.xml")]
		[InlineData(CineworldLists.CinemasUk, "/syndication/cinemas.xml")]
		[InlineData(CineworldLists.FilmTimesEire, "/syndication/film_times_ie.xml")]
		[InlineData(CineworldLists.FilmTimesUk, "/syndication/film_times.xml")]
		[InlineData(CineworldLists.ListingsEire, "/syndication/listings_ie.xml")]
		[InlineData(CineworldLists.ListingsUk, "/syndication/listings.xml")]
		[InlineData(CineworldLists.WeeklyFilmTimesEire, "/syndication/weekly_film_times_ie.xml")]
		[InlineData(CineworldLists.WeeklyFilmTimesUk, "/syndication/weekly_film_times.xml")]
		public void CineworldListsToRelativeUrlStringTests_BehavesPredictably(
			CineworldLists lists,
			string expected)
		{
			Assert.Equal(
				expected,
				lists.ToRelativeUrlString());
		}
	}
}
