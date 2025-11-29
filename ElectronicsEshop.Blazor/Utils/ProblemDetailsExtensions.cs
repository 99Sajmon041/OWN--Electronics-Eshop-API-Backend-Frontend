using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Utils;

public static class ProblemDetailsExtensions
{
    public static async Task<string> ReadProblemMessageAsync(this HttpResponseMessage response, string defaultMessage)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            if (!string.IsNullOrWhiteSpace(problem?.Detail))
                return problem.Detail;
            if (!string.IsNullOrWhiteSpace(problem?.Title))
                return problem.Title;
        }
        catch
        {
        }

        return defaultMessage;
    }
}
