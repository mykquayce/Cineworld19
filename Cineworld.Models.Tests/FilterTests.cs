using Cineworld.Models.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace Cineworld.Models.Tests
{
	public class FilterTests
    {
        [Theory]
        [InlineData(@"{""FilterType"": ""CinemaId"", ""Value"": 18}")]
        public void FilterTests_Deserialization(string json)
        {
            var actual = JsonConvert.DeserializeObject<Filter>(json);

            Assert.NotNull(actual);
            Assert.Equal(FilterTypes.CinemaId, actual.FilterType);
			Assert.Equal("18", actual.Value);
        }
    }
}
