using ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.UpdateAccount;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Models.Mappers;

public static class ApplicationUserMapper
{
    public static UpdateAccountModel ToUpdateModel(this ApplicationUserModel model)
    {
        return new UpdateAccountModel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            DateOfBirth = model.DateOfBirth,
            PhoneNumber = model.PhoneNumber,
            Address = new AddressModel
            {
                Street = model.Address.Street,
                Town = model.Address.Town,
                PostalCode = model.Address.PostalCode,
                NumberOfHouse = model.Address.NumberOfHouse
            }
        };
    }
}
