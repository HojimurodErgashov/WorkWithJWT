using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User> LoginAsync(string login, string password, bool tracking, CancellationToken cancellationToken = default);
        List<User> GetAll();
        Task<User> GetById(Guid id, bool tracking, CancellationToken cancellationToken);
        Task<User> DeleteAsync(Guid id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user); // Id userni ichida kevotti,
    }
}