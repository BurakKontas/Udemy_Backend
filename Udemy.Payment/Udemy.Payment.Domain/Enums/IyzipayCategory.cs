namespace Udemy.Payment.Domain.Enums;

public class IyzipayCategory(string primary, string secondary)
{
    public string Primary { get; } = primary;
    public string Secondary { get; } = secondary;

    public static IyzipayCategory Course => new("Education", "Online Course");
    public static IyzipayCategory Book => new("Education", "Book");
    public static IyzipayCategory Software => new("Software", "Development");
    public static IyzipayCategory Service => new("Service", "Development");
    public static IyzipayCategory NA => new("N/A", "N/A");
}