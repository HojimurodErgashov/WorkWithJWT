using Entities.Model;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User> LoginAsync(string login, string password, bool tracking, CancellationToken cancellationToken = default);

    }
}