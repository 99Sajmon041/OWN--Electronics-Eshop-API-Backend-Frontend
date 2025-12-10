using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Utils;

public static class OrderStatusHelper
{
    public static Dictionary<OrderStatus, string> StatusLabels()
    {
        return new()
        {
            { OrderStatus.Pending, "Čeká na vyřízení" },
            { OrderStatus.Paid, "Zaplaceno" },
            { OrderStatus.Shipped, "Odesláno" },
            { OrderStatus.Completed, "Dokončeno" },
            { OrderStatus.Cancelled, "Zrušeno" }
        };
    }

    public static string GetCzechOrderStatusName(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "Čeká na vyřízení",
            OrderStatus.Paid => "Zaplaceno",
            OrderStatus.Shipped => "Odesláno",
            OrderStatus.Completed => "Dokončeno",
            OrderStatus.Cancelled => "Zrušeno",
            _ => "bg-secondary"
        };
    }

    public static string GetStatusBadgeClass(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "bg-warning text-dark",
            OrderStatus.Paid => "bg-info text-dark",
            OrderStatus.Shipped => "bg-primary",
            OrderStatus.Completed => "bg-success",
            OrderStatus.Cancelled => "bg-danger",
            _ => "bg-secondary"
        };
    }
}
