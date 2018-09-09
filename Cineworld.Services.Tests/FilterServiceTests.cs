using Cineworld.Models;
using Cineworld.Models.Configuration;
using Cineworld.Services.Concrete;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cineworld.Services.Tests
{
	public class FilterServiceTests
    {
		private readonly IReadOnlyCollection<showType> _shows;
		private readonly IReadOnlyCollection<filmType> _films;
		private readonly IReadOnlyCollection<cinemaType> _cinemas;

		public FilterServiceTests()
		{
			_shows = new[]
			{
				new showType { time = new DateTime(1980, 1, 1, 18, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 1, 19, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 1, 20, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 1, 21, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 1, 22, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 2, 18, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 2, 19, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 2, 20, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 2, 21, 0, 0, DateTimeKind.Local), },
				new showType { time = new DateTime(1980, 1, 2, 22, 0, 0, DateTimeKind.Local), },
			};

			_films = new[]
			{
				new filmType { title = "Close Encounters of the Third Kind", },
				new filmType { title = "Jaws", },
				new filmType { title = "Star Wars", },
			};

			_cinemas = new[]
			{
				new cinemaType { id = 1, },
				new cinemaType { id = 2, },
				new cinemaType { id = 3, },
			};
		}

		[Theory]
		[InlineData(new string[0], new DayOfWeek[0], 10)]
		[InlineData(new[] { ">=21:00", }, new[] { DayOfWeek.Wednesday, }, 2)]
		[InlineData(new[] { "<18:00", }, new[] { DayOfWeek.Tuesday, }, 0)]
		[InlineData(new[] { "=18:00", }, new[] { DayOfWeek.Tuesday, }, 1)]
		[InlineData(new[] { "18:00", }, new[] { DayOfWeek.Tuesday, DayOfWeek.Wednesday, }, 2)]
		[InlineData(new[] { ">=18:00", }, new[] { DayOfWeek.Tuesday, }, 5)]
		[InlineData(new[] { ">=18:00", }, new[] { DayOfWeek.Wednesday, }, 5)]
		[InlineData(new string[0], new[] { DayOfWeek.Wednesday, }, 5)]
		[InlineData(new string[0], new[] { DayOfWeek.Tuesday, DayOfWeek.Wednesday, }, 10)]
		public void FilterServiceTests_FilterShows(
			string[] timeFiltersStrings, DayOfWeek[] daysOfWeek, int expectedCount)
		{
			var timeFilters = timeFiltersStrings.Select(s => new TimeFilter(s)).ToList();

			// Act
			var actual = FilterService.Filter(_shows, timeFilters, daysOfWeek).ToList();

			// Assert
			Assert.Equal(expectedCount, actual.Count);
			Assert.All(actual, Assert.NotNull);
			Assert.All(actual, s => Assert.NotEqual(default, s.time));
		}

		[Theory]
		[InlineData(new string[0], 3)]
		[InlineData(new[] { "Jaws", }, 1)]
		[InlineData(new[] { "jaws", }, 1)]
		[InlineData(new[] { "jaw", }, 1)]
		[InlineData(new[] { "a", }, 2)]
		public void FilterServiceTests_FilterFilms(
			string[] titleFilters, int expectedCount)
		{
			// Act
			var actual = FilterService.Filter(_films, titleFilters).ToList();

			// Assert
			Assert.Equal(expectedCount, actual.Count);
			Assert.All(actual, Assert.NotNull);
			Assert.All(actual, s => Assert.NotNull(s.title));
			Assert.All(actual, s => Assert.NotEmpty(s.title));
		}

		[Theory]
		[InlineData(new short[0], 3)]
		[InlineData(new short[] { 1, }, 1)]
		[InlineData(new short[] { 2, }, 1)]
		[InlineData(new short[] { 1, 2, }, 2)]
		[InlineData(new short[] { 1, 2, 3, }, 3)]
		public void FilterServiceTests_FilterCinemas(
			short[] cinemaIds, int expectedCount)
		{
			// Act
			var actual = FilterService.Filter(_cinemas, cinemaIds).ToList();

			// Assert
			Assert.Equal(expectedCount, actual.Count);
			Assert.All(actual, Assert.NotNull);
			Assert.All(actual, c => Assert.InRange(c.id, 1, int.MaxValue));
		}

		[Fact]
		public void FilterServiceTests_()
		{
			var filterses = new FilterCollectionCollection
			{
				new FilterCollection
				{
					new Filter { FilterType = FilterTypes.CinemaId, Value = "1", },
				},
			};

			var filtersesOptionsMock = new Mock<IOptions<FilterCollectionCollection>>();

			filtersesOptionsMock
				.Setup(x => x.Value)
				.Returns(filterses);

			var service = new FilterService(filtersesOptionsMock.Object, new SerializationService());

			var cinemas = new cinemasType
			{
				cinema = new[]
				{
					new cinemaType
					{
						id = 1,
						listing = new[]
						{
							new filmType
							{
								title = "Jaws",
								shows = new[]
								{
									new showType { time = new DateTime(1980, 1, 1, 20, 0, 0, DateTimeKind.Local), },
								},
							},
						},
					},
				},
			};


			// Act
			var actual = service.Filter(cinemas);

			// Assert
			Assert.NotNull(actual);
			Assert.NotNull(actual.cinema);
			Assert.NotEmpty(actual.cinema);
			Assert.Single(actual.cinema);
			Assert.NotNull(actual.cinema[0]);
		}
    }
}
