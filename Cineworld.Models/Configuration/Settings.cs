using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Cineworld.Models.Configuration
{
	public class Settings : INotifyPropertyChanged, IDisposable
	{
		private string _filesPattern;
		private string _filesQuery;
		private string _path;

		public Settings()
		{
			PropertyChanged += Settings_PropertyChanged;
		}

		public string FilesPattern
		{
			get => _filesPattern;
			set
			{
				if (string.Equals(_filesPattern, value)) return;
				_filesPattern = value;
				NotifyPropertyChanged();
			}
		}

		public string FilesQuery
		{
			get { return _filesQuery; }
			set
			{
				if (string.Equals(_filesQuery, value)) return;
				_filesQuery = value;
				NotifyPropertyChanged();
			}
		}

		public string Path
		{
			get { return _path; }
			set
			{
				if (string.Equals(_path, value)) return;
				_path = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#region INotifyPropertyChanged implementation
		private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!(sender is Settings settings))
			{
				return;
			}

			switch(e.PropertyName)
			{
				case nameof(Settings.FilesPattern):
					if (string.IsNullOrWhiteSpace(settings.FilesPattern))
					{
						throw new ArgumentNullException(nameof(Settings.FilesPattern));
					}
					if (settings.FilesPattern.IndexOf("(?<Ticks>", StringComparison.InvariantCulture) < 0)
					{
						throw new ArgumentOutOfRangeException(
							nameof(Settings.FilesPattern),
							settings.FilesPattern,
							$@"{nameof(Settings.FilesPattern)} must contain a capture group called ""Ticks""")
						{
							Data = { { nameof(Settings.FilesPattern), settings.FilesPattern }, },
						};
					}
					break;
				case nameof(Settings.Path):
					if (string.IsNullOrWhiteSpace(settings.Path))
					{
						throw new ArgumentNullException(nameof(Settings.Path));
					}
					break;
				case nameof(Settings.FilesQuery):
					if (string.IsNullOrWhiteSpace(settings.FilesQuery))
					{
						throw new ArgumentNullException(nameof(Settings.FilesQuery));
					}
					break;
			}
		}

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = default(string))
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		#endregion INotifyPropertyChanged implementation

		#region IDisposable implementation
		public void Dispose()
		{
			PropertyChanged -= Settings_PropertyChanged;
		}
		#endregion IDisposable implementation
	}
}
