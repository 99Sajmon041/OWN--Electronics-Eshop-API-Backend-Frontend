using Microsoft.AspNetCore.Identity;

namespace ElectronicsEshop.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public bool Active { get; set; } = true;
    public Address Address { get; set; } = default!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public Cart Cart { get; set; } = default!;
}
