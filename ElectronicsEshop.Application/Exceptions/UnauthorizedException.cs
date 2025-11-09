namespace ElectronicsEshop.Application.Exceptions;

public class UnauthorizedException(string? message = "Unauthorized.") : Exception(message)
{
}
