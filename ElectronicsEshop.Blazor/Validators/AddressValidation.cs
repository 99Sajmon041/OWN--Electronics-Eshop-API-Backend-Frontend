using ElectronicsEshop.Blazor.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Validators
{
    public static class AddressValidation
    {
        public static List<ValidationResult> Validate(AddressModel? address)
        {
            var results = new List<ValidationResult>();

            if(address is null)
            {
                results.Add(new ValidationResult("Chybí adresa."));
                return results;
            }

            if (string.IsNullOrWhiteSpace(address.Street))
                results.Add(new ValidationResult("Chybí ulice."));

            if (string.IsNullOrWhiteSpace(address.NumberOfHouse))
                results.Add(new ValidationResult("Chybí popisné."));

            if (string.IsNullOrWhiteSpace(address.PostalCode))
                results.Add(new ValidationResult("Chybí PSČ."));

            if (string.IsNullOrWhiteSpace(address.Town))
                results.Add(new ValidationResult("Chybí město."));

            return results;
        }

        public static List<ValidationResult> ValidateAddress(this IHasAddress model)
        {
            return Validate(model.Address);
        }
    }
}
