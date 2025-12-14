namespace ElectronicsEshop.Blazor.Models.Orders.Admin.GetAdminOrder
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
