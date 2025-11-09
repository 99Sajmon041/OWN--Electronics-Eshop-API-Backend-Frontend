using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Infrastructure.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Claims;

namespace ElectronicsEshop.Infrastructure.Security;

public class EshopUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole> (userManager, roleManager, options) 
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser applicationUser)
    {
        var identity = await base.GenerateClaimsAsync(applicationUser);

        if(applicationUser.DateOfBirth != default)
        {
            var dob = applicationUser.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            identity.AddClaim(new Claim(AppClaimTypes.DateOfBirth, dob, ClaimValueTypes.String));
        }

        return identity;
    }
}
    