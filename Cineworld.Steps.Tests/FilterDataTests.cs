using Cineworld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Interface;
using Xunit;

namespace Cineworld.Steps.Tests
{
	public class TestData2
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
						new Predicate<showType>[]
						{
							(showType s) => s.time.Date == new DateTime(1980, 1, 1),
						},
					},
					new object[]
					{
						new[]
						{
							new DateTime(1980, 1, 1, 20, 0, 0),
							new DateTime(1980, 1, 2, 20, 0, 0),
						},
						new Predicate<showType>[]
						{
							(showType s) => s.time.Date == new DateTime(1980, 1, 2),
						},
					},
					new object[]
					{
						new[]
						{
							new DateTime(1980, 1, 1, 20, 0, 0),
							new DateTime(1980, 1, 1, 21, 0, 0),
						},
						new Predicate<showType>[]
						{
							(showType s) => s.time.TimeOfDay > new TimeSpan(20, 30, 0),
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
						new Predicate<showType>[]
						{
							(showType s) => s.time.Date == new DateTime(1980, 1, 1),
							(showType s) => s.time.TimeOfDay > new TimeSpan(20, 30, 0),
						},
					},
				};
			}
		}
	}

	public class FilterDataTests
	{
		private readonly Steps.FilterData _filterDataStep;

		public FilterDataTests()
		{
			var serializationService = new Services.Concrete.SerializationService();
			_filterDataStep = new Steps.FilterData(serializationService);
		}

		[Theory]
		[MemberData(memberName: nameof(TestData2.Times), MemberType = typeof(TestData2))]
		public void FilterDataTests_OneCinemaOneFilmTwoDays_FilteredToJustOneDay(
			IEnumerable<DateTime> times,
			IEnumerable<Predicate<showType>> filters)
		{
			// Arrange
			_filterDataStep.Original = new cinemasType
			{
				cinema = new[]
				{
					new cinemaType
					{
						listing = new []
						{
							new filmType
							{
								shows = times.Select(dt => new showType { time = dt, }).ToArray(),
							},
						},
					},
				},
			};

			_filterDataStep.ShowFilters = filters;

			// Act
			_filterDataStep.Run(default(IStepExecutionContext));

			// Arrange
			var actual = _filterDataStep.Filtered;

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
			Assert.Single(actual.cinema[0].listing[0].shows);

			Assert.NotNull(actual.cinema[0].listing[0].shows[0]);
			Assert.NotEqual(default(DateTime), actual.cinema[0].listing[0].shows[0].time);
		}
	}
}
