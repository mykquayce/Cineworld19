using Cineworld.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using Xunit;

namespace Cineworld.Steps.Tests
{
	public class OutputTests
	{
		[Fact]
		public void OutputTests_DatesAreFormattedOkay()
		{
			var cinemas = new cinemasType
			{
				cinema = new[]
				{
					new cinemaType
					{
						name = "cinema_name",
						listing = new []
						{
							new filmType
							{
								title = "film_title",
								shows = new[]
								{
									new showType
									{
										time = new DateTime(1980, 4, 13, 21, 30, 0, DateTimeKind.Utc),
									},
									new showType
									{
										time = new DateTime(1980, 4, 13, 23, 30, 0, DateTimeKind.Utc),
									},
								},
							},
						},
					},
				},
			};

			var strings = new List<string>();

			var step = new Output
			{
				Cinemas = cinemas,
				Outputs = new Action<string>[] { strings.Add, },
			};

			var result = step.Run(default(IStepExecutionContext));

			Assert.NotEmpty(strings);
			Assert.Equal(2, strings.Count);
			Assert.Equal("cinema_name 1980-04-13 21:30 : film_title", strings[0]);
			Assert.Equal("cinema_name 1980-04-13 23:30 : film_title", strings[1]);
		}
	}
}
