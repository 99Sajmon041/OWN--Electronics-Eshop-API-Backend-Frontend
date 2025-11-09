namespace ElectronicsEshop.Application.Exceptions;

public class ValidationException(IDictionary<string, string[]> errors) : Exception("Validation failed.")
{
    public IDictionary<string, string[]> Errors { get; } = errors;
}
