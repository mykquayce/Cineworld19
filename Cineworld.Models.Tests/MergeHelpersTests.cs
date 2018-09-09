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

		[Fact]
		public void MergeHelpersTests_Films()
		{
			var filmses = new[]
			{
				new[]
				{
					new filmType
					{
						edi = 1,
						title = "Jaws",
						shows = new[] { new showType { time = new DateTime(1980, 1, 1, 20, 0, 0, DateTimeKind.Local), }, },
					},
					new filmType
					{
						edi = 1,
						title = "Jaws",
						shows = new[] { new showType { time = new DateTime(1980, 1, 1, 21, 0, 0, DateTimeKind.Local), }, },
					},
				},
			};

			// Act
			var actual = MergeHelpers.MergeFilms(filmses).ToList();

            // Assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Single(actual);
			Assert.All(actual, Assert.NotNull);
			Assert.All(actual, f => Assert.NotNull(f.shows));
			Assert.All(actual, f => Assert.NotEmpty(f.shows));
			Assert.Equal(2, actual[0].shows.Length);
			Assert.Equal(new DateTime(1980, 1, 1, 20, 0, 0, DateTimeKind.Local), actual[0].shows[0].time);
			Assert.Equal(new DateTime(1980, 1, 1, 21, 0, 0, DateTimeKind.Local), actual[0].shows[1].time);
		}
    }
}
