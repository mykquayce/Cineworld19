using System;
using System.Linq;
using Xunit;

namespace Cineworld.Models.Tests
{
	public class cinemasTypeTests
	{
		private readonly cinemasType _data = new cinemasType
		{
			cinema = new[]
			{
				new cinemaType
				{
					name = "Cineworld Ashton-under-Lyne",
					listing = new filmType[]
					{
						new filmType
						{
							title = "(2D) Ant-Man And The Wasp",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 18, 0, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 19, 20, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 50, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "Christopher Robin",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 20, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "Incredibles 2",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 19, 50, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "Mamma Mia! Here We Go Again",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 18, 30, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 10, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 21, 10, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "Mission : Impossible - Fallout",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 20, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "The Darkest Minds",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 0, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "(IMAX) The Equalizer 2",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 19, 40, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "The Equalizer 2",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 18, 0, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 50, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "The Festival",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 18, 10, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 19, 0, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 40, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 21, 30, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "The Meg",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 18, 20, 0, DateTimeKind.Unspecified),
								},
								new showType
								{
									time = new DateTime(2018, 8, 17, 21, 0, 0, DateTimeKind.Unspecified),
								},
							},
						},
						new filmType
						{
							title = "Unfriended: Dark Web",
							shows = new[]
							{
								new showType
								{
									time = new DateTime(2018, 8, 17, 20, 30, 0, DateTimeKind.Unspecified),
								},
							},
						},
					}
				},
			},
		};

		[Fact]
		public void cinemasTypeTests_ToStrings_BehavesPredictably()
		{
			// Act
			var strings = _data.ToStrings().ToList();

			// Assert
			Assert.NotNull(strings);
			Assert.NotEmpty(strings);
			Assert.Equal(21, strings.Count);
			Assert.Equal("Cineworld Ashton-under-Lyne", strings[0]);
			Assert.Equal("Cineworld Ashton-under-Lyne", strings[0]);
			Assert.Equal("2018-08-17 18:00 : (2D) Ant-Man And The Wasp", strings[1]);
			Assert.Equal("2018-08-17 18:00 : The Equalizer 2", strings[2]);
			Assert.Equal("2018-08-17 18:10 : The Festival", strings[3]);
			Assert.Equal("2018-08-17 18:20 : The Meg", strings[4]);
			Assert.Equal("2018-08-17 18:30 : Mamma Mia! Here We Go Again", strings[5]);
			Assert.Equal("2018-08-17 19:00 : The Festival", strings[6]);
			Assert.Equal("2018-08-17 19:20 : (2D) Ant-Man And The Wasp", strings[7]);
			Assert.Equal("2018-08-17 19:40 : (IMAX) The Equalizer 2", strings[8]);
			Assert.Equal("2018-08-17 19:50 : Incredibles 2", strings[9]);
			Assert.Equal("2018-08-17 20:00 : The Darkest Minds", strings[10]);
			Assert.Equal("2018-08-17 20:10 : Mamma Mia! Here We Go Again", strings[11]);
			Assert.Equal("2018-08-17 20:20 : Christopher Robin", strings[12]);
			Assert.Equal("2018-08-17 20:20 : Mission : Impossible - Fallout", strings[13]);
			Assert.Equal("2018-08-17 20:30 : Unfriended: Dark Web", strings[14]);
			Assert.Equal("2018-08-17 20:40 : The Festival", strings[15]);
			Assert.Equal("2018-08-17 20:50 : (2D) Ant-Man And The Wasp", strings[16]);
			Assert.Equal("2018-08-17 20:50 : The Equalizer 2", strings[17]);
			Assert.Equal("2018-08-17 21:00 : The Meg", strings[18]);
			Assert.Equal("2018-08-17 21:10 : Mamma Mia! Here We Go Again", strings[19]);
			Assert.Equal("2018-08-17 21:30 : The Festival", strings[20]);
		}

		[Fact]
		public void cinemasTypeTests_ToString_BehavesPredictably()
		{
			// Act
			var s = _data.ToString();

			// Assert
			Assert.NotNull(s);
			Assert.NotEmpty(s);
			Assert.Equal(@"Cineworld Ashton-under-Lyne
2018-08-17 18:00 : (2D) Ant-Man And The Wasp
2018-08-17 18:00 : The Equalizer 2
2018-08-17 18:10 : The Festival
2018-08-17 18:20 : The Meg
2018-08-17 18:30 : Mamma Mia! Here We Go Again
2018-08-17 19:00 : The Festival
2018-08-17 19:20 : (2D) Ant-Man And The Wasp
2018-08-17 19:40 : (IMAX) The Equalizer 2
2018-08-17 19:50 : Incredibles 2
2018-08-17 20:00 : The Darkest Minds
2018-08-17 20:10 : Mamma Mia! Here We Go Again
2018-08-17 20:20 : Christopher Robin
2018-08-17 20:20 : Mission : Impossible - Fallout
2018-08-17 20:30 : Unfriended: Dark Web
2018-08-17 20:40 : The Festival
2018-08-17 20:50 : (2D) Ant-Man And The Wasp
2018-08-17 20:50 : The Equalizer 2
2018-08-17 21:00 : The Meg
2018-08-17 21:10 : Mamma Mia! Here We Go Again
2018-08-17 21:30 : The Festival", s);
		}
	}
}
