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

			_cinemas = new cinemasType
			{
				cinema = new[]
				{
					new cinemaType
					{
						id = 21,
						listing = new[]
						{
							new filmType
							{
								title = "Jaws",
								shows = new[]
								{
									new showType
									{
										time = new DateTime(2018, 8, 16, 17, 0, 0, DateTimeKind.Unspecified),
									},
								},
							},
						},
					},
				},
			};
        }

        [Fact]
        public void FilterServiceTests_BehavesPredictably()
        {
			var filters = new FilterCollection
			{
				new Filter { FilterType = FilterTypes.CinemaId, Value = 21, },
				new Filter { FilterType = FilterTypes.DayOfWeek, Value = "Thursday",},
				new Filter { FilterType = FilterTypes.Title, Value="jaw", },
			};

            var filterOptions = Moq.Mock.Of<IOptions<FilterCollection>>(o => o.Value == filters);

            var service = new Services.Concrete.FilterService(filterOptions, _serializationService);

            // Act
            var after = service.Filter(_cinemas);

            // Assert
            Assert.NotNull(after);
            Assert.NotNull(after.cinema);
            Assert.NotEmpty(after.cinema);
			Assert.All(after.cinema, Assert.NotNull);
			Assert.All(after.cinema.Select(c => c.listing), Assert.NotNull);
			Assert.All(after.cinema.Select(c => c.listing), Assert.NotEmpty);
        }
    }
}
