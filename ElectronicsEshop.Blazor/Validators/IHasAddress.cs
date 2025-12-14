using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Validators;

public interface IHasAddress
{
    AddressModel Address { get; set; }
}
