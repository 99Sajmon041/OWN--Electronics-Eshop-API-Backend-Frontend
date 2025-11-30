using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Auth.ForgotPassword;

public sealed class ForgotPasswordModel
{
    [Required(ErrorMessage = "E-mail je povinný údaj.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string Email { get; set; } = string.Empty;
}
