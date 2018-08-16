using Cineworld.Models;
using Cineworld.Models.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Cineworld.Services.Tests
{
    public class FilterServiceTests
    {
        private readonly Services.ISerializationService _serializationService;
        private readonly cinemasType _cinemas;

        public FilterServiceTests()
        {
            _serializationService = new Services.Concrete.SerializationService();

            _cinemas = new cinemasType
            {
                cinema = new[]
                {
                    new cinemaType
                    {
                        id = 21,
                        listing = new []
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
            var filters = new List<Filter>
            {
                new Filter
                {
                    CinemaIds = { 21, },
                    DaysOfWeek = { DayOfWeek.Thursday, },
                    TitlePattern = "jaw",
                },
            };

            var filterOptions = Moq.Mock.Of<IOptions<List<Filter>>>(o => o.Value == filters);

            var service = new Services.Concrete.FilterService(filterOptions, _serializationService);

            // Act
            var after = service.Filter(_cinemas).ToList();

            // Assert
            Assert.NotNull(after);
            Assert.NotEmpty(after);
        }
    }
}
