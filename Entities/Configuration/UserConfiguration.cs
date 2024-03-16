using Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = new Guid("80f7f49b-e5f2-4248-ab3a-349da4390553"),
                    FirstName = "Hojimurod",
                    LastName = "Ergashov",
                    Login = "admin123",
                    Password = "admin123",
                    Role = RoleEnum.Admin,
                    IsDeleted = false
                },
                new User
                {
                    Id = new Guid("36929717-b202-479b-aa74-6cf59e59cadf"),
                    FirstName = "Rakhmatjon",
                    LastName = "Khamidov",
                    Login = "user1234",
                    Password = "user1234",
                    Role = RoleEnum.User,
                    IsDeleted = false
                });
        }
    }
}
