using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;
using ElectronicsEshop.Infrastructure.Database;
using ElectronicsEshop.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ElectronicsEshop.Infrastructure.Seeders;

public sealed class DefaultDataSeeder(AppDbContext db, UserManager<ApplicationUser> users, RoleManager<IdentityRole> roles, IOptions<SeedOptions> seedOptions) : IDefaultDataSeeder
{
    string SeedEmail = seedOptions.Value.AdminEmail;
    string SeedPassword = seedOptions.Value.AdminPassword;
    string seedRole = seedOptions.Value.AdminRole;

    public async Task SeedData()
    {
        await EnsureRoleAsync(UserRoles.Admin.ToString());
        await EnsureRoleAsync(UserRoles.Client.ToString());
        await EnsureRoleAsync(UserRoles.User.ToString());

        var user = await users.FindByEmailAsync(SeedEmail);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = SeedEmail,
                Email = SeedEmail,
                EmailConfirmed = true,
                PhoneNumber = "+420 756987453",
                FirstName = "Petr",
                LastName = "Kořínský",
                DateOfBirth = new DateOnly(1995, 5, 23),
                Address = new Address
                {
                    Street = "Hlavní",
                    NumberOfHouse = "123",
                    PostalCode = "11000",
                    Town = "Praha"
                }
            };

            await users.CreateAsync(user, SeedPassword);
        }

        if (!await users.IsInRoleAsync(user, seedRole))
        {
             await users.AddToRoleAsync(user, seedRole);
        }

        var hasCart = await db.Carts.AnyAsync(c => c.ApplicationUserId == user.Id);
        if (!hasCart)
        {
            db.Carts.Add(new Cart
            {
                ApplicationUserId = user.Id,
                UpdatedAt = DateTime.UtcNow,
                CartItems = new List<CartItem>()
            });
            await db.SaveChangesAsync();
        }
    }

    private async Task EnsureRoleAsync(string roleName)
    {
        if (!await roles.RoleExistsAsync(roleName))
        {
            await roles.CreateAsync(new IdentityRole(roleName));
        }
    }
}
