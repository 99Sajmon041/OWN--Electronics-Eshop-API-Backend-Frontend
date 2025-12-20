namespace ElectronicsEshop.Blazor.Utils;

public sealed class UiState
{
    public bool IsBusy { get; private set; }
    public string? BusyText { get; private set; }

    public event Action? OnChange;

    public void StartBusy(string text)
    {
        IsBusy = true;
        BusyText = text;
        OnChange?.Invoke();
    }

    public void StopBusy()
    {
        IsBusy = false;
        BusyText = null;
        OnChange?.Invoke();
    }
}
