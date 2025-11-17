namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface ICartRepository
{
    Task CreateAsync(string userId, CancellationToken cancellationToken);
}
