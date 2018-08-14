using System;
using System.IO;
using System.Threading.Tasks;

namespace Cineworld.Clients
{
    public interface ICineworldClient
    {
		Task<Stream> DownloadAllPerformancesEireAsync();
		Task<Stream> DownloadAllPerformancesUkAsync();
		Task<Stream> DownloadCinemasEireAsync();
		Task<Stream> DownloadCinemasUkAsync();
		Task<Stream> DownloadFilmTimesEireAsync();
		Task<Stream> DownloadFilmTimesUkAsync();
		Task<Stream> DownloadListingsEireAsync();
		Task<Stream> DownloadListingsUkAsync();
		Task<Stream> DownloadWeeklyFilmTimesEireAsync();
		Task<Stream> DownloadWeeklyFilmTimesUkAsync();

		Task<Stream> DownloadAsync(Models.CineworldLists list);


		Task<DateTime> GetAllPerformancesEireLastModifiedAsync();
		Task<DateTime> GetAllPerformancesUkLastModifiedAsync();
		Task<DateTime> GetCinemasEireLastModifiedAsync();
		Task<DateTime> GetCinemasUkLastModifiedAsync();
		Task<DateTime> GetFilmTimesEireLastModifiedAsync();
		Task<DateTime> GetFilmTimesUkLastModifiedAsync();
		Task<DateTime> GetListingsEireLastModifiedAsync();
		Task<DateTime> GetListingsUkLastModifiedAsync();
		Task<DateTime> GetWeeklyFilmTimesEireLastModifiedAsync();
		Task<DateTime> GetWeeklyFilmTimesUkLastModifiedAsync();

		Task<DateTime> GetLastModifiedAsync(Models.CineworldLists list);
    }
}
