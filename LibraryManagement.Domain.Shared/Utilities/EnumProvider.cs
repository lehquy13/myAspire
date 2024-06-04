using LibraryManagement.Domain.Shared.Enums;

namespace LibraryManagement.Domain.Shared.Utilities;

public static class EnumProvider
{
    public static List<string> Genre { get; } = Enum.GetNames(typeof(Genre)).ToList();
    
    public static List<string> PaymentMethods { get; } = Enum.GetNames(typeof(PaymentMethod)).ToList();
}