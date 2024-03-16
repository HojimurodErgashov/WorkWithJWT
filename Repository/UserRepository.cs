    using Entities;
using Entities.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext):base(repositoryContext)
        {
           
        }

        public async Task<User> LoginAsync(string login, string password, bool tracking, CancellationToken cancellationToken = default) =>
                                   await FindByCondition(x => x.Login.Equals(login) && x.Password.Equals(password), tracking)
                                            .SingleOrDefaultAsync(cancellationToken);

    }
}
