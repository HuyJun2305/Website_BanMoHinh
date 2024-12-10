
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace XuongTT_API.Model
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            //static Roles
            List<IdentityRole<Guid>> roles = new List<IdentityRole<Guid>>()
            {
                new IdentityRole<Guid>{Id = Guid.NewGuid(), Name = "Admin", NormalizedName="ADMIN"},
                new IdentityRole<Guid>{Id = Guid.NewGuid(), Name = "Staff", NormalizedName="STAFF"},
                new IdentityRole<Guid>{Id = Guid.NewGuid(), Name = "Customer", NormalizedName="CUSTOMER"},
            };
            builder.Entity<IdentityRole<Guid>>().HasData(roles);
            //static Users
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    Name = "khoong",
                    PhoneNumber = "chua co",
                    Addresses = null,
                    ImgUrl = "",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "staff",
                    NormalizedUserName = "STAFF",
                    Email = "staff@gmail.com",
                    NormalizedEmail = "STAFF@GMAIL.COM",
                    Name = "khoong",
                    PhoneNumber = "chua co",
                    Addresses = null,
                    ImgUrl = "",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "user1",
                    NormalizedUserName = "USER1",
                    Email = "user1@gmail.com",
                    NormalizedEmail = "USER1@GMAIL.COM",
                    Name = "khoong",
                    PhoneNumber = "chua co",
                    Addresses = null,
                    ImgUrl = "",
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            };

            builder.Entity<ApplicationUser>().HasData(users);

            // static userRole

            List<IdentityUserRole<Guid>> userRoles = new List<IdentityUserRole<Guid>>();
                users[0].PasswordHash = passwordHasher.HashPassword(users[0], "Pa$$word1");
                users[1].PasswordHash = passwordHasher.HashPassword(users[1], "Pa$$word1");
                users[2].PasswordHash = passwordHasher.HashPassword(users[2], "Pa$$word1");
                
                userRoles.Add(new IdentityUserRole<Guid> { UserId = users[0].Id,RoleId = roles.First(q => q.Name == "Admin").Id });
                userRoles.Add(new IdentityUserRole<Guid> { UserId = users[0].Id,RoleId = roles.First(q => q.Name == "Staff").Id });
                userRoles.Add(new IdentityUserRole<Guid> { UserId = users[1].Id,RoleId = roles.First(q => q.Name == "Staff").Id });
                userRoles.Add(new IdentityUserRole<Guid> { UserId = users[2].Id,RoleId = roles.First(q => q.Name == "Customer").Id });

            builder.Entity<IdentityUserRole<Guid>>().HasData(userRoles);
        }
    }
}
