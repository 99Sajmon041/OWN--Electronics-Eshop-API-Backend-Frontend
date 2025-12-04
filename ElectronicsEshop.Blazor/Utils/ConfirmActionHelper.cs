using Microsoft.JSInterop;

namespace ElectronicsEshop.Blazor.Utils;

public static class ConfirmActionHelper
{
    public static async Task<bool> ConfirmAction(this IJSRuntime JS, string outputText)
    {
        var confirmed = await JS.InvokeAsync<bool>("confirm", outputText);

        if (!confirmed)
        {
            return false;
        }

        return true;
    }
}
