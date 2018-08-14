using System.Collections.Generic;

namespace Cineworld.Models.Configuration
{
	public class ApiSettings
	{
		public string CineworldUrl { get; set; }
		public string SlackUrl { get; set; }
		public ICollection<string> SlackWebhook { get; } = new List<string>();
	}
}
