namespace Udemy.Common.Events.Payment.EventDtos;

public record CardInformationEventDto(string CardHolderName, string CardNumber, int ExpireMonth, int ExpireYear, int Cvc, string? Email);