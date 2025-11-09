namespace ElectronicsEshop.Application.Exceptions;

public class NotFoundException(string resource, string key) : Exception($"{resource} ({key}) was not found.")
{
}
