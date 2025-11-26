namespace ElectronicsEshop.Application.Exceptions;

public class NotFoundException(string resource, object key) : Exception($"{resource} s ({key}) nenalezen / o.")
{
}
