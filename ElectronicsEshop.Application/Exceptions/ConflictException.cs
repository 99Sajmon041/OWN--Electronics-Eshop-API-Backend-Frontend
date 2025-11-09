namespace ElectronicsEshop.Application.Exceptions;

public class ConflictException(string? message = "Conflict occured.") : Exception(message)
{
}
