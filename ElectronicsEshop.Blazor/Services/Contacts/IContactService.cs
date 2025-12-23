using ElectronicsEshop.Blazor.Models.Contacts;

namespace ElectronicsEshop.Blazor.Services.Contacts;

public interface IContactService
{
    Task SendMessageToSupportAsync(ContactModel model, CancellationToken ct = default);
}
