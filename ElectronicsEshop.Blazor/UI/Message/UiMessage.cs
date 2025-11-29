namespace ElectronicsEshop.Blazor.UI.Message;

public sealed class UiMessage(MessageType type, string text)
{
    public MessageType Type { get; } = type;
    public string Text { get; } = text;
}
