using Microsoft.JSInterop;

namespace ElectronicsEshop.Blazor.Utils;

public static class ConfirmActionHelper
{
    public static async Task<bool> ConfirmAction(this IJSRuntime JS, string outputText)
    {
        return await JS.InvokeAsync<bool>("confirm", outputText);
    }

    public static Task<bool> ConfirmWarning(this IJSRuntime JS, string text)
    {
        return JS.ConfirmAction($"⚠️ {text}");
    }
}
