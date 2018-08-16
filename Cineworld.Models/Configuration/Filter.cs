using System;
using System.Collections.Generic;
using System.Text;

namespace Cineworld.Models.Configuration
{
    public class Filter
    {
        public ICollection<short> CinemaIds { get; } = new List<short>();
        public ICollection<DayOfWeek> DaysOfWeek { get; } = new List<DayOfWeek>();
        public string TitlePattern { get; set; }
    }
}
