namespace ElectronicsEshop.Blazor.UI.State;

public sealed class CartState
{
    public int ItemsCount { get; private set; }

    public event Action? OnChange;

    public void SetCount(int count)
    {
        count = Math.Max(0, count);

        if (ItemsCount == count)
            return;

        ItemsCount = count;
        OnChange?.Invoke();
    }
    public void Add(int delta)
    {
        if (delta <= 0) return;
        SetCount(ItemsCount + delta);
    }

    public void Subtract(int delta)
    {
        if (delta <= 0) return;
        SetCount(ItemsCount - delta);
    }

    public void Clear()
    {
        if (ItemsCount == 0)
            return;

        ItemsCount = 0;
        OnChange?.Invoke();
    }
}
