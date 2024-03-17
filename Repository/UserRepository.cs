using Entities;
using Entities.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task<User> LoginAsync(string login, string password, bool tracking, CancellationToken cancellationToken = default) =>
                                   await FindByCondition(x => x.Login.Equals(login) && x.Password.Equals(password), tracking)
                                            .SingleOrDefaultAsync(cancellationToken);

        public async Task<User> DeleteAsync(Guid id)
        {
            var user = await FindByCondition(x => x.Id.Equals(id), false).SingleOrDefaultAsync();

            if (user != null)
            {
                Delete(user);
                return user;
            }

            return null;
        }

        public Task<User> UpdateAsync(User user)
        {
            Update(user);
            return Task.FromResult(user);
        }

        public async Task<User> CreateAsync(User user)
        {
          return await Create(user);
        }

        public List<User> GetAll()
        {
           List<User> users =  FindAll(false).ToList<User>();
            return users;
        }

        public async Task<User> GetById(Guid id, bool tracking , CancellationToken cancellationToken)
        {
           return await FindByCondition(p => p.Id == id, tracking).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
