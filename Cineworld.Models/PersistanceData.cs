using System;

namespace Cineworld.Models
{
	public class PersistanceData
	{
		public cinemasType ShowingsFromRemote { get; set; }
		public cinemasType FilteredShowingsFromLocal { get; set; }
		public cinemasType FilteredShowingsFromRemote { get; set; }
		public DateTime? LastModifiedFromLocal { get; set; }
		public DateTime LastModifiedFromRemote { get; set; }
		public bool? HaveNewData { get; set; }
	}
}
