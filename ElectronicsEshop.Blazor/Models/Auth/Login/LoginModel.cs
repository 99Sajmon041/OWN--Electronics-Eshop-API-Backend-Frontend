using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Auth.Login;

public sealed class LoginModel
{
    [Required(ErrorMessage = "E-mail je povinný údaj.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinné.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}