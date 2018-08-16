using Newtonsoft.Json;
using Xunit;

namespace Cineworld.Models.Tests
{
    public class FiltersTests
    {
        [Theory]
        [InlineData(@"{""cinemaIds"": [1,2], ""daysOfWeek"": [""Friday""], ""titlePattern"": ""(unlimited|preview)""}")]
        public void FiltersTests_DeserializesFromJson(string json)
        {
            var filters = JsonConvert.DeserializeObject<Models.Configuration.Filter>(json);

        }
    }
}
