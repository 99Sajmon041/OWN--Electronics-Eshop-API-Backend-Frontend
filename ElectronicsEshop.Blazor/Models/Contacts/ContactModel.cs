using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Contacts;

public sealed class ContactModel
{
    [Required(ErrorMessage = "Zadejte e-mail.")]
    [EmailAddress(ErrorMessage = "Neplatný formát e-mailu.")]
    public string ReplyEmail { get; set; } = string.Empty;

    [MinLength(10)]
    [Required(ErrorMessage = "Zadejte předmět.")]
    [StringLength(120, ErrorMessage = "Předmět musí mít 10 - 120 znaků.")]
    public string Subject { get; set; } = string.Empty;

    [MinLength(10)]
    [Required(ErrorMessage = "Napište zprávu.")]
    [StringLength(2000, ErrorMessage = "Zpráva musí mít 10 - 2000 znaků.")]
    public string Message { get; set; } = string.Empty;
}
