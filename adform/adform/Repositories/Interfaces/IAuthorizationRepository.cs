using adform.Models;
using System.Threading.Tasks;

namespace adform.Repositories.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<ReceivedToken> Authorize(string id, string secret);
    }
}
