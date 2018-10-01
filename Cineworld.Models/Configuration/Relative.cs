using System;

namespace Cineworld.Models.Configuration
{
    [Flags]
    public enum Relative : byte
    {
        None = 0,
        LessThan = 1,
        LessThanOrEqualTo = 2,
        EqualTo = 4,
        NotEqualTo = 8,
        GreaterThanOrEqualTo = 16,
        GreaterThan = 32,
    }
}
