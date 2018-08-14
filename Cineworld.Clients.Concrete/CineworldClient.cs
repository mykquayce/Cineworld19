using Cineworld.Models;
using Cineworld.Models.Helpers;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cineworld.Clients.Concrete
{
	public class CineworldClient : ClientBase, ICineworldClient
	{
		public CineworldClient(IHttpClientFactory httpClientFactory)
			: base(httpClientFactory)
		{ }

		public Task<Stream> DownloadAllPerformancesEireAsync() => DownloadAsync(CineworldLists.AllPerformancesEire);
		public Task<Stream> DownloadAllPerformancesUkAsync() => DownloadAsync(CineworldLists.AllPerformancesUk);
		public Task<Stream> DownloadCinemasEireAsync() => DownloadAsync(CineworldLists.CinemasEire);
		public Task<Stream> DownloadCinemasUkAsync() => DownloadAsync(CineworldLists.CinemasUk);
		public Task<Stream> DownloadFilmTimesEireAsync() => DownloadAsync(CineworldLists.FilmTimesEire);
		public Task<Stream> DownloadFilmTimesUkAsync() => DownloadAsync(CineworldLists.FilmTimesUk);
		public Task<Stream> DownloadListingsEireAsync() => DownloadAsync(CineworldLists.ListingsEire);
		public Task<Stream> DownloadListingsUkAsync() => DownloadAsync(CineworldLists.ListingsUk);
		public Task<Stream> DownloadWeeklyFilmTimesEireAsync() => DownloadAsync(CineworldLists.WeeklyFilmTimesEire);
		public Task<Stream> DownloadWeeklyFilmTimesUkAsync() => DownloadAsync(CineworldLists.WeeklyFilmTimesUk);

		public Task<Stream> DownloadAsync(CineworldLists list) => base.DownloadFileAsync(list.ToRelativeUrlString());

		public Task<DateTime> GetAllPerformancesEireLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.AllPerformancesEire);
		public Task<DateTime> GetAllPerformancesUkLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.AllPerformancesUk);
		public Task<DateTime> GetCinemasEireLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.CinemasEire);
		public Task<DateTime> GetCinemasUkLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.CinemasUk);
		public Task<DateTime> GetFilmTimesEireLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.FilmTimesEire);
		public Task<DateTime> GetFilmTimesUkLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.FilmTimesUk);
		public Task<DateTime> GetListingsEireLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.ListingsEire);
		public Task<DateTime> GetListingsUkLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.ListingsUk);
		public Task<DateTime> GetWeeklyFilmTimesEireLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.WeeklyFilmTimesEire);
		public Task<DateTime> GetWeeklyFilmTimesUkLastModifiedAsync() => GetLastModifiedAsync(CineworldLists.WeeklyFilmTimesUk);

		public Task<DateTime> GetLastModifiedAsync(CineworldLists list) => base.GetLastModifiedAsync(list.ToRelativeUrlString());
	}
}
