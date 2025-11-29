using System.Security.Claims;
using System.Text;
using ElectronicsEshop.Application.Authorization.DTOs;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Globalization;

namespace ElectronicsEshop.Application.Authorization.Commands.Login;

public sealed class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IConfiguration configuration,
    ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            logger.LogWarning("Pokus o přihlášení s neexistujícím e-mailem: {Email}", request.Email);
            throw new DomainException("Neplatné přihlašovací údaje.");
        }

        if (!user.Active)
        {
            logger.LogWarning("Pokus o přihlášení deaktivovaného uživatele: {Email}", request.Email);
            throw new DomainException("Účet je deaktivován.");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                logger.LogWarning("Uživatel {Email} je uzamčen.", request.Email);
                throw new DomainException("Účet je dočasně zablokován. Zkuste to prosím později.");
            }

            logger.LogWarning("Neplatné přihlašovací údaje pro uživatele {Email}.", request.Email);
            throw new DomainException("Neplatné přihlašovací údaje.");
        }

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Email ?? string.Empty)
        };

        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if(user.DateOfBirth != default)
        {
            var dob = user.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            claims.Add(new Claim(ClaimTypes.DateOfBirth, dob));
        }

        var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");

        var issuer = configuration["Jwt:Issuer"] ?? "ElectronicsEshop";
        var audience = configuration["Jwt:Audience"] ?? "ElectronicsEshopClient";
        var expiresMinutes = int.TryParse(configuration["Jwt:ExpiresMinutes"], out var minutes) ? minutes : 60;

        var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        logger.LogInformation("Uživatel {Email} byl úspěšně přihlášen.", request.Email);

        return new LoginResult
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
        };
    }
}
