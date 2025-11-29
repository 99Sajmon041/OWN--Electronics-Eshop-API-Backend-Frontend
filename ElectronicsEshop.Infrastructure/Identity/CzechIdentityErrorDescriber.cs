using Microsoft.AspNetCore.Identity;

namespace ElectronicsEshop.Infrastructure.Identity;

public class CzechIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateEmail(string email)
    {
        return new()
        {
            Code = nameof(DuplicateEmail),
            Description = $"Účet s e-mailem '{email}' už existuje."
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new()
        {
            Code = nameof(DuplicateUserName),
            Description = $"Uživatelské jméno '{userName}' je již obsazeno."
        };
    }
}
