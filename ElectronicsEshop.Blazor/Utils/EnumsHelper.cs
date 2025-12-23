using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Utils;

public static class EnumsHelper
{
    public static Dictionary<OrderStatus, string> StatusLabels()
    {
        return new()
        {
            { OrderStatus.Pending, "Čeká na vyřízení" },
            { OrderStatus.Paid, "Zaplaceno" },
            { OrderStatus.Shipped, "Odesláno" },
            { OrderStatus.Completed, "Dokončeno" },
            { OrderStatus.Canceled, "Zrušeno" }
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
            OrderStatus.Canceled => "Zrušeno",
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
            OrderStatus.Canceled => "bg-danger",
            _ => "bg-secondary"
        };
    }

    public static string GetCzechPaymentStatusName(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Succeeded => "Úspěšný",
            PaymentStatus.Failed => "Neúspěšný",
            _ => "Neznámý"
        };
    }
}
