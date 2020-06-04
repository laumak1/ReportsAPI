using adform.Models;
using adform.Repositories.Interfaces;
using adform.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace adform.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _repository;

        public ReportsService(IReportsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<WeekData>> GetWeeks(string token)
        {
            var report = await _repository.GetReports(token);
            var rows = ParseData(report);
            
            if(rows.Count > 0)
            {
                rows = FillWithMissingDates(rows);
            }
            
            var weeklydaata = GetDataByWeek(rows);
            
            return weeklydaata;
        }

        public async Task<string> testavimas()
        {
            return "veikia viskas";
        }

        public async Task<List<string>> GetAnomalies(string token)
        {
            var report = await _repository.GetReports(token);
            var rows = ParseData(report);

            if (rows.Count > 0)
            {
                rows = FillWithMissingDates(rows);
            }

            var result = FindAnomalies(rows);

            return result;
        }

        private ICollection<Row> ParseData(ReportResult report)
        {
            Collection<Row> rows = new Collection<Row>();
            foreach (var item in report.reportData.rows)
            {
                rows.Add(new Row
                {
                    date = DateTime.Parse(item[0]),
                    count = Int16.Parse(item[1])
                });
            }

            return rows;
        }

        private ICollection<WeekData> GetDataByWeek(ICollection<Row> rows)
        {
            var result =
                rows.GroupBy(x => CultureInfo.CurrentCulture.DateTimeFormat.Calendar
                        .GetWeekOfYear(x.date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new WeekData
                {
                    week = g.Key,
                    bidRequests = g.Select(b => b.count).Sum()
                })
                .ToList<WeekData>();

            return result;
        }


        private List<string> FindAnomalies(ICollection<Row> rows)
        {
            List<string> dates = new List<string>();
            if(rows.Count > 0)
            {
                int prevCount = rows.FirstOrDefault().count;

                foreach (var row in rows)
                {
                    if (prevCount != row.count &&
                        (prevCount == 0 || row.count == 0)
                        )
                    {
                        dates.Add(ConvertDateToString(row.date));
                    }
                    else if (
                        (double)prevCount / (double)row.count >= 3 ||
                        (double)row.count / (double)prevCount >= 3)
                    {
                        dates.Add(ConvertDateToString(row.date));
                    }
                    prevCount = row.count;
                }
            }

            return dates;
        }

        private string ConvertDateToString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        private ICollection<Row> FillWithMissingDates(ICollection<Row> rows)
        {
            var year = rows.FirstOrDefault().date.Year;
            
            int daysCount = DateTime.IsLeapYear(year) ? 366 : 365;
            DateTime startDate = DateTime.Parse( (year + 1) + "-01-01");

            var dates = Enumerable.Range(1, daysCount)
                        .Select(offset => startDate.AddDays(-offset).Date)
                        .OrderBy(x => x.Date);

            var result = from date in dates
                         join tmp in rows on date equals tmp.date into g
                         from gr in g.DefaultIfEmpty()
                         select new Row
                         {
                            count = gr == null ? 0 : gr.count,
                            date = date
                         };

            return result.ToList();
        }

    }
}
