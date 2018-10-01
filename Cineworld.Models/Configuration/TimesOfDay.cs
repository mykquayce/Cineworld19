using System;

namespace Cineworld.Models.Configuration
{
    [Flags]
    public enum TimesOfDay : byte
    {
        None = 0,
        Afternoon = 1,
        Evening = 2,
        Morning = 4,
        Night = 8,

        AM = Night | Morning,
        PM = Afternoon | Evening,

        AllDay = Afternoon | Evening | Morning | Night,
    }
}
