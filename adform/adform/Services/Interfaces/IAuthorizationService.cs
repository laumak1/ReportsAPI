using System.Threading.Tasks;

namespace adform.Services
{
    public interface IAuthorizationService
    {
        Task<string> GetToken();
    }
}
