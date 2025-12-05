namespace ElectronicsEshop.Blazor.UI.Message;

public sealed class MessageService
{
    public UiMessage? Current { get; private set; }
    public event Action? OnChange;

    private int _version;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public void ShowSuccess(string text) => Show(MessageType.Success, text);
    public void ShowError(string text) => Show(MessageType.Error, text);
    public void ShowInfo(string text) => Show(MessageType.Info, text);

    private void Show(MessageType type, string text)
    {
        Current = new UiMessage(type, text);

        _version++;
        var currentVersion = _version;

        NotifyStateChanged();

        _ = AutoClearAsycnc(currentVersion);
    }

    private async Task AutoClearAsycnc(int version)
    {
        await Task.Delay(3000);

        if(version == _version)
        {
            Clear();
        }
    }

    public void Clear()
    {
        Current = null;
        NotifyStateChanged();
    }
}
