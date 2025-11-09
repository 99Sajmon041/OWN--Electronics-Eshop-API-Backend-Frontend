namespace ElectronicsEshop.Application.User;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
