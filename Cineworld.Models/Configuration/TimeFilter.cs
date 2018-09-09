using Dawn;
using System;
using System.Text.RegularExpressions;

namespace Cineworld.Models.Configuration
{
    public class TimeFilter
    {
        private static readonly string _pattern = @"^(?<Relative>[<>!=]{0,2})(?<Time>\d{2}:\d{2})$";
        private static readonly Regex _regex = new Regex(_pattern);

        public TimeFilter(string s)
        {
            Guard.Argument(() => s).NotNull().NotEmpty().Matches(_pattern);

            var match = _regex.Match(s);

            Relative = ParseRelative(match.Groups["Relative"].Value);
            Time = TimeSpan.Parse(match.Groups["Time"].Value);
        }

        public Relative Relative { get; set; }
        public TimeSpan Time { get; set; }

        public Func<TimeSpan, bool> Func
        {
            get
            {
                switch (Relative)
                {
                    case Relative.LessThan: return ts => ts < Time;
                    case Relative.LessThanOrEqualTo: return ts => ts <= Time;
                    case Relative.EqualTo: return ts => ts == Time;
                    case Relative.NotEqualTo: return ts => ts != Time;
                    case Relative.GreaterThanOrEqualTo: return ts => ts >= Time;
                    case Relative.GreaterThan: return ts => ts > Time;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static Relative ParseRelative(string s)
        {
            Guard.Argument(() => s).NotNull().LengthInRange(0, 2);

            switch (s)
            {
                case "<": return Relative.LessThan;
                case "<=": return Relative.LessThanOrEqualTo;
                case "":
                case "=":
                case "==":
                    return Relative.EqualTo;
                case "<>":
                case "!=":
                    return Relative.NotEqualTo;
                case ">=": return Relative.GreaterThanOrEqualTo;
                case ">": return Relative.GreaterThan;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, $"Unexpected value of {nameof(s)}: {s}")
                    {
                        Data = { { nameof(s), s }, },
                    };
            }
        }
    }
}
