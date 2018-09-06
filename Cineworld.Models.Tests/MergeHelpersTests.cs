using Cineworld.Models.Helpers;
using System;
using System.Linq;
using Xunit;

namespace Cineworld.Models.Tests
{
    public class MergeHelpersTests
    {
        [Theory]
        [InlineData(new[] { 1, 2, 3, }, new[] { 2, 3, 4, }, new[] { 3, 4, 5, })]
        [InlineData(new[] { 3, 2, 1, }, new[] { 2, 3, 4, }, new[] { 3, 4, 5, })]
        [InlineData(new[] { 3, 2, 1, }, new[] { 4, }, new[] { 3, 4, 5, })]
        public void MergeHelpersTests_ThreeIntArrays(int[] one, int[] two, int[] three)
        {
            var actual = MergeHelpers
                .Merge(one, two, three)
                .OrderBy(i => i)
                .ToList();

            Assert.Equal(new[] { 1, 2, 3, 4, 5, }, actual);
        }

        [Fact]
        public void MergeHelpersTests_Shows()
        {
            // Arrange
            var one = new[]
            {
                new showType { time = new DateTime(1980, 1, 1, 19, 0, 0, DateTimeKind.Local), },
                new showType { time = new DateTime(1980, 1, 1, 20, 0, 0, DateTimeKind.Local), },
                new showType { time = new DateTime(1980, 1, 1, 21, 0, 0, DateTimeKind.Local), },
                new showType { time = new DateTime(1980, 1, 1, 23, 0, 0, DateTimeKind.Local), },
            };

            var two = new[]
            {
                new showType { time = new DateTime(1980, 1, 1, 19, 0, 0, DateTimeKind.Local), },
            };

            var three = new[]
            {
                new showType { time = new DateTime(1980, 1, 1, 22, 0, 0, DateTimeKind.Local), },
                new showType { time = new DateTime(1980, 1, 1, 23, 0, 0, DateTimeKind.Local), },
            };

            // Act
            var actual = MergeHelpers.MergeShows(one, two, three).ToList();

            // Assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Equal(5, actual.Count);
        }
    }
}
