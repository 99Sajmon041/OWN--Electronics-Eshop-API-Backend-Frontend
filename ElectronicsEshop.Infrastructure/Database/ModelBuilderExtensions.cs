using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElectronicsEshop.Infrastructure.Database;

public static class ModelBuilderExtensions
{
    public static void ConvertsAllEnumsToStrings(this ModelBuilder modelbuilder, int len = 64)
    {
        foreach(var entityType in modelbuilder.Model.GetEntityTypes())
        {
            foreach(var property in entityType.GetProperties())
            {
                var clr = property.ClrType;
                var enumType = clr.IsEnum ? clr : (Nullable.GetUnderlyingType(clr)?.IsEnum == true ? Nullable.GetUnderlyingType(clr)! : null);

                if (enumType == null) continue;

                var converterType = typeof(EnumToStringConverter<>).MakeGenericType(enumType);
                var converter = (ValueConverter)Activator.CreateInstance(converterType)!;

                property.SetValueConverter(converter);
                property.SetColumnType($"nvarchar({len})");
                property.SetMaxLength(len);
                property.SetIsUnicode(true);
            }
        }
    }
}
