using Cineworld.Models;
using Cineworld.Models.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Interface;
using Xunit;

namespace Cineworld.Steps.Tests
{
	public static class Data
	{
		public static IEnumerable<object[]> Times
		{
			get
			{
				return new[]
				{
					new object[]
					{
						new[]
						{
							new DateTime(1980, 1, 1, 20, 0, 0),
							new DateTime(1980, 1, 2, 20, 0, 0),
						},
					},
					new object[]
					{
						new[]
						{
							new DateTime(1980, 1, 1, 20, 0, 0),
							new DateTime(1980, 1, 1, 21, 0, 0),
						},
					},
					new object[]
					{
						new[]
						{
							new DateTime(1980, 1, 1, 20, 0, 0),
							new DateTime(1980, 1, 1, 21, 0, 0),
							new DateTime(1980, 1, 2, 20, 0, 0),
							new DateTime(1980, 1, 2, 21, 0, 0),
						},
					},
				};
			}
		}
	}

	public class FilterDataTests
	{
		private readonly Steps.FilterData _step;

		public FilterDataTests()
		{
			var filters = new FilterCollection
			{
				new Filter { FilterType = FilterTypes.DayOfWeek, Value = "Tuesday", },
			};

            var filterOptions = Moq.Mock.Of<IOptions<FilterCollection>>(o => o.Value == filters);
			var serializationService = new Services.Concrete.SerializationService();
			var filterService = new Services.Concrete.FilterService(filterOptions, serializationService);
			_step = new Steps.FilterData(filterService);
		}

		[Theory]
		[MemberData(memberName: nameof(Data.Times), MemberType = typeof(Data))]
		public void FilterDataTests_OneCinemaOneFilmTwoDays_FilteredToJustOneDay(
			IEnumerable<DateTime> times)
		{
			// Arrange
			_step.Original = new cinemasType
			{
				cinema = new[]
				{
					new cinemaType
					{
						listing = new[]
						{
							new filmType
							{
								shows = times.Select(dt => new showType { time = dt, }).ToArray(),
							},
						},
					},
				},
			};

			// Act
			_step.Run(default(IStepExecutionContext));

			// Arrange
			var actual = _step.Filtered;

			// Assert
			Assert.NotNull(actual);
			Assert.NotNull(actual.cinema);
			Assert.NotEmpty(actual.cinema);
			Assert.Single(actual.cinema);

			Assert.NotNull(actual.cinema[0]);
			Assert.NotNull(actual.cinema[0].listing);
			Assert.NotEmpty(actual.cinema[0].listing);
			Assert.Single(actual.cinema[0].listing);

			Assert.NotNull(actual.cinema[0].listing[0]);
			Assert.NotNull(actual.cinema[0].listing[0].shows);
			Assert.NotEmpty(actual.cinema[0].listing[0].shows);

			Assert.NotNull(actual.cinema[0].listing[0].shows[0]);
			Assert.NotEqual(default(DateTime), actual.cinema[0].listing[0].shows[0].time);
		}
	}
}
