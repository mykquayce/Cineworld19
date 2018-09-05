using Cineworld.Models;
using Cineworld.Models.Configuration;
using Cineworld.Services.Concrete;
using System;
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

        [Theory]
        [InlineData(new[] { "jaw", }, 1)]
        [InlineData(new[] { "(jaw|shin)", }, 2)]
        [InlineData(new[] { "(jaw|shin|gho)", }, 3)]
        [InlineData(new[] { "jaw", "shin", "gho", }, 3)]
        public void FilterServiceTests_FilterFilms(
            string[] titlePatterns, int expectedCount)
        {
            var before = new[]
            {
                new filmType{ title = "Jaws", },
                new filmType{ title = "The Shining", },
                new filmType{ title = "Ghostbusters", },
            };

            var after = FilterService.FilterFilms(before, titlePatterns).ToList();

            // Assert
            Assert.NotNull(after);
            Assert.Equal(expectedCount, after.Count);
        }

        [Theory]
        [InlineData(new string[0], 4)]
        [InlineData(new[] { ">7pm", }, 3)]
        public void FilterServiceTests_FilterShows(string[] timeFilterStrings, int expectedCount)
        {
            // Arrange
            var before = new[]
            {
                new showType { time = new DateTime(1980, 1, 1, 19, 0, 0, DateTimeKind.Local), },
                new showType { time = new DateTime(1980, 1, 1, 20, 0, 0, DateTimeKind.Local), },
                new showType { time = new DateTime(1980, 1, 1, 21, 0, 0, DateTimeKind.Local), },
                new showType { time = new DateTime(1980, 1, 1, 22, 0, 0, DateTimeKind.Local), },
            };

            var timeFilters = timeFilterStrings.Select(s => new TimeFilter(s)).ToList();

            // Act
            var after = FilterService.FilterShows(before, timeFilters, new DayOfWeek[0]).ToList();

            // Assert
            Assert.NotNull(after);
            Assert.Equal(expectedCount, after.Count);
        }
    }
}
