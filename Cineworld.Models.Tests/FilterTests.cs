using Cineworld.Models.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using Xunit;

namespace Cineworld.Models.Tests
{
	public class FilterTests
    {
        [Theory]
        [InlineData("{\"CinemaIds\": [ 18, 23 ],\"DaysOfWeek\": [ \"Friday\" ],\"TimesOfDay\": [ \"Evening\", \"Night\" ]}")]
        public void FilterTests_Deserialization(string json)
        {
            var actual = JsonConvert.DeserializeObject<Filter>(json);

            Assert.NotNull(actual);

            Assert.NotNull(actual.CinemaIds);
            Assert.NotEmpty(actual.CinemaIds);
            Assert.Equal(2, actual.CinemaIds.Count);
            Assert.Equal(18, actual.CinemaIds.First());
            Assert.Equal(23, actual.CinemaIds.Skip(1).First());

            Assert.NotNull(actual.DaysOfWeek);
            Assert.NotEmpty(actual.DaysOfWeek);
            Assert.Single(actual.DaysOfWeek);
            Assert.Equal(DayOfWeek.Friday, actual.DaysOfWeek.First());

            Assert.NotNull(actual.TimesOfDays);
            Assert.NotEmpty(actual.TimesOfDays);
            Assert.Equal(2, actual.TimesOfDays.Count);
            Assert.Equal(TimesOfDay.Evening, actual.TimesOfDays.First());
            Assert.Equal(TimesOfDay.Night, actual.TimesOfDays.Skip(1).First());

            Assert.NotNull(actual.TimesOfDay);
            Assert.Equal(TimesOfDay.Evening | TimesOfDay.Night, actual.TimesOfDay);
        }
    }
}
