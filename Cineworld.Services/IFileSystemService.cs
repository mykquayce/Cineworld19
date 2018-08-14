using Cineworld.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cineworld.Services
{
	public interface IFileSystemService
	{
		DateTime? GetShowingsLastModified();
		cinemasType LoadLastestShowings();
		Task SaveShowingsAsync(cinemasType cinemas, DateTime lastModified);
		Task SaveShowingsAsync(cinemasType cinemas, FileInfo destinationFile);
		Task SaveShowingsAsync(cinemasType cinemas, Stream destinationStream);
	}
}
