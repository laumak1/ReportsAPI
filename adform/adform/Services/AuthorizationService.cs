using adform.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace adform.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthorizationService(
            IAuthorizationRepository repository,
            IConfiguration configuration
            )
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<string> GetToken()
        {
            var token = await _repository.Authorize(
                _configuration["Authorization:client_id"],
                _configuration["Authorization:client_secret"]
                );
            
            return token.access_token;
        }
    }
}
