using Cineworld.Models;
using System;
using System.Collections.Generic;
using WorkflowCore.Interface;
using Xunit;

namespace Cineworld.Steps.Tests
{
	public static class TestData
	{
		private static readonly cinemasType _showings1 = new cinemasType
		{
			cinema = new[]
			{
				new cinemaType
				{
					id = 23,
					listing = new[]
					{
						new filmType
						{
							title = "FIlm Name",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(1980, 1, 1, 12, 0, 0, DateTimeKind.Utc),
								},
							},
						},
					},
				},
			},
		};

		private static readonly cinemasType _showings2 = new cinemasType
		{
			cinema = new[]
			{
				new cinemaType
				{
					id = 23,
					listing = new[]
					{
						new filmType
						{
							title = "Other FIlm Name",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(1980, 1, 1, 12, 0, 0, DateTimeKind.Utc),
								},
							},
						},
					},
				},
			},
		};

		public static IEnumerable<object[]> Data
		{
			get
			{
				return new []
				{
					new object[] { default(cinemasType), default(cinemasType), false, },
					new object[] { _showings1, default(cinemasType), false, },
					new object[] { default(cinemasType), _showings1, false, },
					new object[] { _showings1, _showings1, true, },
					new object[] { _showings2, _showings2, true, },
					new object[] { _showings1, _showings2, false, },
					new object[] { _showings2, _showings1, false, },
				};
			}
		}
	}

	public class CompareDataTests
	{
		private readonly Steps.CompareData _step;

		public CompareDataTests()
		{
			_step = new Steps.CompareData();
		}

		[Theory]
		[MemberData(memberName: nameof(TestData.Data), MemberType = typeof(TestData))]
		public void CompareDataTests_OneNull_BehavesPredictably(cinemasType local, cinemasType remote, bool expected)
		{
			// Arrange
			_step.Local = local;
			_step.Remote = remote;

			// Act
			_step.RunAsync(default(IStepExecutionContext)).ConfigureAwait(false).GetAwaiter().GetResult();

			// Assert
			Assert.NotNull(_step.AreIdentical);
			Assert.Equal(expected, _step.AreIdentical);
		}
	}
}
