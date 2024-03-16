using Entities.Configuration;
using Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext:DbContext
    {

        public RepositoryContext(DbContextOptions options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<User> User { get; set; }
    }
}
