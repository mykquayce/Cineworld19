using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cineworld.Models.Configuration
{
    public class Filter
	{
        public ICollection<short> CinemaIds { get; set; }
        public ICollection<DayOfWeek> DaysOfWeek { get; } = new List<DayOfWeek>();
        [JsonProperty("TimesOfDay")]
        public ICollection<TimesOfDay> TimesOfDays { get; } = new List<TimesOfDay>();
        public ICollection<string> TitlePatterns { get; } = new List<string>();

        [JsonIgnore]
        public TimesOfDay TimesOfDay => TimesOfDays.Aggregate(TimesOfDay.None, (sum, next) => sum |= next);
	}
}
