using Cineworld.Models.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cineworld.Models.Tests
{
    public class FilterTests
    {
        [Theory]
        [InlineData(@"{""cinemaIds"": [ 18, 21 ]}")]
        public void FilterTests_Deserialization(string json)
        {
            var actual = JsonConvert.DeserializeObject<Filter>(json);

            Assert.NotNull(actual);
            Assert.NotNull(actual.CinemaIds);
            Assert.NotEmpty(actual.CinemaIds);
            Assert.Equal(2, actual.CinemaIds.Count);
            Assert.Equal(new List<short> { 18, 21, }, actual.CinemaIds);
            Assert.NotNull(actual.DaysOfWeek);
            Assert.Empty(actual.DaysOfWeek);
            Assert.NotNull(actual.TimeFilters);
            Assert.Empty(actual.TimeFilters);
            Assert.NotNull(actual.TitlePatterns);
            Assert.Empty(actual.TitlePatterns);
        }
    }
}
