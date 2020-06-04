using Xunit;
using Moq;
using adform.Repositories.Interfaces;
using adform.Models;
using adform.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Tests
{
    public class ReportsSetUp
    {
        public ReportResult reportResult;

        public ReportsSetUp()
        {
            reportResult = new ReportResult
            {
                reportData = new ReportData
                {
                    columnHeaders = new List<string>
                    {
                        "date",
                        "bidRequests"
                    },
                    rows = new List<List<string>>
                    {
                        new List<string>
                        {
                            "2019-01-01T00:00:00",
                            "300"
                        },
                        new List<string>
                        {
                            "2019-01-06T00:00:00",
                            "300"
                        },
                        new List<string>
                        {
                            "2019-01-07T00:00:00",
                            "50"
                        },
                        new List<string>
                        {
                            "2019-01-14T00:00:00",
                            "300"
                        }
                    }
                },
                correlationCode = "testData"
            };

        }
    }

    public class ReportsTests
    {
        private readonly ReportsService _reportsService;

        public ReportsTests()
        {
            ReportsSetUp setUp = new ReportsSetUp();

            var reportsRepository = new Mock<IReportsRepository>();
            reportsRepository.Setup(x => x.GetReports("")).Returns(Task.FromResult(setUp.reportResult));

            _reportsService = new ReportsService(reportsRepository.Object);
        }

        [Theory]
        [InlineData(1, 600)]
        [InlineData(2, 50)]
        [InlineData(5, 0)]
        public void When_GettingDataAggregatedByWeeks_Expect_ReturnsExpectedValues(int weekNumber, int expectedCount)
        {
            var result = _reportsService.GetWeeks("").Result;

            int week = result.ElementAt(weekNumber - 1).week;
            int requests = result.ElementAt(weekNumber - 1).bidRequests;

            Assert.Equal(expectedCount, requests);
        }

        [Theory]
        [InlineData("2019-01-02", "2019-01-06", "2019-01-07", "2019-01-08", "2019-01-14", "2019-01-15")]
        public void When_GettingDataAnomalies_Expect_ReturnsAllAnomalies(params string[] dates)
        {
            var result = _reportsService.GetAnomalies("").Result;

            var datesList = dates.ToList<string>();

            int compared1To2 = result.Except(datesList).Count();
            int compared2To1 = datesList.Except(result).Count();

            Assert.Equal(0, compared1To2);
            Assert.Equal(0, compared2To1);
        }
    }
}
