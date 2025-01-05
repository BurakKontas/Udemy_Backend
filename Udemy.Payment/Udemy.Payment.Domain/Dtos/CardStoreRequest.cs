namespace Udemy.Payment.Domain.Dtos;

public class CardStoreRequest
{
    public string CardHolderName { get; set; }
    public string CardNumber { get; set; }
    public string ExpireMonth { get; set; }
    public string ExpireYear { get; set; }
    public string UserKey { get; set; }
    public string? Email { get; set; }
}