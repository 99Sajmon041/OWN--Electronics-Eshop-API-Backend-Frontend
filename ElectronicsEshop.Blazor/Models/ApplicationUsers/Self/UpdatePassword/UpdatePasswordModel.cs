using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.UpdatePassword;

public sealed class UpdatePasswordModel
{
    [Required(ErrorMessage = "Stávající heslo je povinné.")]
    public string OldPassword { get; set; } = default!;

    [Required(ErrorMessage = "Nové heslo je povinné.")]
    public string NewPassword { get; set; } = default!;

    [Required(ErrorMessage = "Potvrzení nového hesla je povinné.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Hesla se neshodují.")]
    public string ConfirmPassword { get; set; } = default!;
}
