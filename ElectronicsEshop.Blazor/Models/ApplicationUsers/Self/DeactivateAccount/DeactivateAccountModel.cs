using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.DeactivateAccount;

public sealed class DeactivateAccountModel
{
    [Required(ErrorMessage = "Heslo je povinné")]
    public string Password { get; set; } = string.Empty;
}
