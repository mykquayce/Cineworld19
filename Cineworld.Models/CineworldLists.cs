using System;

namespace Cineworld.Models
{
	[Flags]
	public enum CineworldLists : byte
	{
		None = 0,

		AllPerformances = 1,
		Cinemas = 2,
		FilmTimes = 4,
		Listings = 8,
		WeeklyFilmTimes = 16,

		Eire = 32,
		Uk = 64,

		AllPerformancesEire = AllPerformances | Eire,
		AllPerformancesUk = AllPerformances | Uk,

		CinemasEire = Cinemas | Eire,
		CinemasUk = Cinemas | Uk,

		FilmTimesEire = FilmTimes | Eire,
		FilmTimesUk = FilmTimes | Uk,

		ListingsEire = Listings | Eire,
		ListingsUk = Listings | Uk,

		WeeklyFilmTimesEire = WeeklyFilmTimes | Eire,
		WeeklyFilmTimesUk = WeeklyFilmTimes | Uk,
	}
}
