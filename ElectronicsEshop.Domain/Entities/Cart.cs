namespace ElectronicsEshop.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = default!;
    public string ApplicationUserId { get; set; } = default!;
    public DateTime UpdatedAt { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
