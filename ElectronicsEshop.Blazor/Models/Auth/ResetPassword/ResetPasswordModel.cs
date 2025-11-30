using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Auth.ResetPassword;

public sealed class ResetPasswordModel
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nové heslo je povinné.")]
    [MinLength(8, ErrorMessage = "Minimální délka hesla je 8 znaků.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Hesla se musí shodovat.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}