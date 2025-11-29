namespace ElectronicsEshop.Blazor.UI.Message;

public sealed class MessageService
{
    public UiMessage? Current { get; private set; }

    public event Action? onChange;

    private void NotifyStateChanged() => onChange?.Invoke();

    public void ShowSuccess(string text)
    {
        Current = new UiMessage(MessageType.Success, text);
        NotifyStateChanged();
    }

    public void ShowError(string text)
    {
        Current = new UiMessage(MessageType.Error, text);
        NotifyStateChanged();
    }

    public void ShowInfo(string text)
    {
        Current = new UiMessage(MessageType.Info, text);
        NotifyStateChanged();
    }

    public void Clear()
    {
        Current = null;
        NotifyStateChanged();
    }
}
