using MediatR;

namespace ElectronicsEshop.Application.Contacts.SendContactMessage;

public sealed class SendContactMessageCommand : IRequest
{
    public string ReplyEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
