using adform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace adform.Services.Interfaces
{
    public interface IReportsService
    {
        Task<List<string>> GetAnomalies(string token);
        Task<ICollection<WeekData>> GetWeeks(string token);
    }
}
