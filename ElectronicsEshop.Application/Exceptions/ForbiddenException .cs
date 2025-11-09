namespace ElectronicsEshop.Application.Exceptions;

public class ForbiddenException(string? message = "Access forbidden.") : Exception(message)
{
}
