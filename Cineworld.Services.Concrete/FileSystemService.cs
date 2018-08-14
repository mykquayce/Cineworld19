﻿using Cineworld.Models.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cineworld.Services.Concrete
{
	public class FileSystemService : IFileSystemService
	{
		private readonly Regex _filesRegex;
		private const RegexOptions _regexOptions = RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;
		private readonly string _filesQuery;
		private readonly DirectoryInfo _rootDirectory;
		private readonly ISerializationService _serializationService;

		#region Constructor
		public FileSystemService(
			IOptions<Settings> settingsOptions,
			ISerializationService serializationService)
		{
			_filesRegex = new Regex(
				settingsOptions?.Value?.FilesPattern ?? throw new ArgumentOutOfRangeException(nameof(Settings.FilesPattern)),
				_regexOptions);

			_filesQuery = settingsOptions?.Value?.FilesQuery ?? throw new ArgumentOutOfRangeException(nameof(Settings.FilesQuery));

			_rootDirectory = new DirectoryInfo(
				(settingsOptions?.Value?.Path ?? throw new ArgumentOutOfRangeException(nameof(Settings.Path)))
				+ Path.DirectorySeparatorChar
				);

			_serializationService = serializationService
				?? throw new ArgumentNullException(nameof(serializationService));
		}
		#endregion Constructor

		public Models.cinemasType LoadLastestShowings()
		{
			var file = GetFiles()?.FirstOrDefault().Item1;

			if (file is default(FileInfo))
			{
				return default(Models.cinemasType);
			}

			using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				if (!stream.CanRead)
				{
					throw new ArgumentOutOfRangeException(nameof(file), file, "Cannot read file")
					{
						Data = { { nameof(file), file.FullName }, },
					};
				}

				return _serializationService.Deserialize<Models.cinemasType>(stream);
			}
		}

		#region Save
		public async Task SaveAsync(string path, Stream source)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				throw new ArgumentNullException(nameof(path));
			}

			var file = new FileInfo(path);

			if (file.Exists)
			{
				throw new ArgumentOutOfRangeException(nameof(path), path, "File already exists")
				{
					Data = { { nameof(path), path }, },
				};
			}

			using (var destination = file.Open(FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				if (!destination.CanWrite)
				{
					throw new ArgumentOutOfRangeException(nameof(path), path, "Cannot write to file")
					{
						Data = { { nameof(path), path }, },
					};
				}

				await source.CopyToAsync(destination).ConfigureAwait(false);
			}
		}
		#endregion Save

		public DateTime? GetShowingsLastModified()
			=> GetFiles()?.FirstOrDefault().Item2;

		public async Task SaveShowingsAsync(Models.cinemasType cinemas, DateTime lastModified)
		{
			if (cinemas is default(Models.cinemasType)) throw new ArgumentNullException(nameof(cinemas));
			if (lastModified == default(DateTime)) throw new ArgumentNullException(nameof(lastModified));

			var destinationFile = new FileInfo($"{_rootDirectory}listings_{lastModified.Ticks:D}.xml");

			await SaveShowingsAsync(cinemas, destinationFile).ConfigureAwait(false);
		}

		public async Task SaveShowingsAsync(Models.cinemasType cinemas, FileInfo destinationFile)
		{
			if (cinemas is default(Models.cinemasType)) throw new ArgumentNullException(nameof(cinemas));
			if (destinationFile.Exists)
			{
				throw new InvalidOperationException("Destination file already exists")
				{
					Data = { { nameof(destinationFile), destinationFile.FullName }, },
				};
			}

			using (var destinationStream = destinationFile.Open(FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				await SaveShowingsAsync(cinemas, destinationStream).ConfigureAwait(false);
			}
		}

		public async Task SaveShowingsAsync(Models.cinemasType cinemas, Stream destinationStream)
		{
			if (!destinationStream.CanWrite)
			{
				throw new InvalidOperationException($"Cannot write to {nameof(destinationStream)}");
			}

			using (var sourceStream = _serializationService.Serialize(cinemas))
			{
				if (!sourceStream.CanRead)
				{
					throw new InvalidOperationException($"Cannot read from {nameof(sourceStream)}");
				}

				await sourceStream.CopyToAsync(destinationStream).ConfigureAwait(false);
			}
		}

		private IEnumerable<(FileInfo, DateTime)> GetFiles()
		{
			return from f in _rootDirectory.GetFiles(_filesQuery)
						let m = _filesRegex.Match(f.Name)
						where m.Success
						let s = m.Groups["Ticks"].Value
						let t = long.Parse(s)
						orderby t descending
						let d = new DateTime(t)
						select (f, d);
		}
	}
}
