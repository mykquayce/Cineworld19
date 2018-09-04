using Cineworld.Models;
using Cineworld.Services.Concrete;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cineworld.Services.Tests
{
    public class FilterServiceTests
    {
        [Theory]
        [InlineData(new short[0], 3)]
        [InlineData(new short[] { 1, }, 1)]
        [InlineData(new short[] { 1, 1, }, 1)]
        [InlineData(new short[] { 2, }, 1)]
        [InlineData(new short[] { 3, }, 1)]
        [InlineData(new short[] { 1, 2, }, 2)]
        [InlineData(new short[] { 3, 2, }, 2)]
        [InlineData(new short[] { 1, 2, 3, }, 3)]
        [InlineData(new short[] { 1, 3, 2, }, 3)]
        [InlineData(new short[] { 4, }, 0)]
        public void FilterServiceTests_FilterCinemas(ICollection<short> cinemaIds, int expectedCount)
        {
            // Arrange
            var before = new[]
            {
                new cinemaType { id = 1, },
                new cinemaType { id = 2, },
                new cinemaType { id = 3, },
            };

            // Act
            var after = FilterService.FilterCinemas(before, cinemaIds).ToList();

            // Assert
            Assert.NotNull(after);
            Assert.Equal(expectedCount, after.Count);
        }
    }
}
