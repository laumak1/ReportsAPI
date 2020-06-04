using System.Threading.Tasks;

namespace adform.Repositories.Interfaces
{
    public interface IReportsRepository
    {
        Task<ReportResult> GetReports(string token);
    }
}
