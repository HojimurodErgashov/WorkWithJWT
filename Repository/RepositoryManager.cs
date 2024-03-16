using Contracts;
using Entities;
using System;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private IUserRepository _userRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }

        public IUserRepository User
        {
            get
            {
                if(_userRepository == null)
                {
                    _userRepository = new UserRepository(_repositoryContext);
                }
                return _userRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
        public void Save() => _repositoryContext.SaveChanges();
    }
}
