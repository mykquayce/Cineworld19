using System;
using System.Collections.Generic;
using System.Text;

namespace Cineworld.Models.Configuration
{
    public class FilterCollection : List<Filter> { }

    public class Filter
    {
        public ICollection<short> CinemaIds { get; } = new List<short>();
        public ICollection<DayOfWeek> DaysOfWeek { get; } = new List<DayOfWeek>();
        public ICollection<string> TimeFilters { get; } = new List<string>();
        public ICollection<string> TitlePatterns { get; } = new List<string>();
    }
}
