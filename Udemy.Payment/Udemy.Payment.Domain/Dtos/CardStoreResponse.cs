namespace Udemy.Payment.Domain.Dtos;

public class CardStoreResponse
{
    public required string CardToken { get; set; }
    public required string CardAlias { get; set; }
    public required string Status { get; set; }
    public required string ErrorMessage { get; set; }
    public required string ExternalId { get; set; }
    public string? Email { get; set; }
}