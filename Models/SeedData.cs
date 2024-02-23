using Mello.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace WebApplication1.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService
                <DbContextOptions<AppDbContext>>()))
            {
                if (context.Roles.Any())
                {
                    return;
                }

                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                        Name = "User",
                        NormalizedName = "User".ToUpper()
                    }
                    );
                var hasher = new PasswordHasher<User>();

                context.Users.AddRange(
                    new User
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb0",
                        UserName = "Admin",
                        EmailConfirmed = true,
                        NormalizedEmail = "ADMIN@MAIL.COM",
                        Email = "admin@mail.com",
                        NormalizedUserName = "ADMIN",
                        PasswordHash = hasher.HashPassword(null, "Admin1!"),
                        BirthDay = DateTime.Now,
                        Date_created = DateTime.Now,
                        PhoneNumber = "071111",
                        PhoneNumberConfirmed = true
                    },
                    new User
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb1",
                        UserName = "Mario",
                        EmailConfirmed = true,
                        NormalizedEmail = "MARIO@MAIL.COM",
                        Email = "mario@mail.com",
                        NormalizedUserName = "MARIO",
                        PasswordHash = hasher.HashPassword(null, "Admin1!"),
                        BirthDay = DateTime.Now,
                        Date_created = DateTime.Now,
                        PhoneNumber = "071111",
                        PhoneNumberConfirmed = true
                    });

                context.UserRoles.AddRange(
                    new IdentityUserRole<string>
                    {
                        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb1"
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
