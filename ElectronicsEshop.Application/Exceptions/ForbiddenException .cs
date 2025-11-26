namespace ElectronicsEshop.Application.Exceptions;

public class ForbiddenException(string? message = "Přístup odepřen.") : Exception(message)
{
}
