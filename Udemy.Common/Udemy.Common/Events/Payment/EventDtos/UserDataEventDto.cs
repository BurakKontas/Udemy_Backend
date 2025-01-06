namespace Udemy.Common.Events.Payment.EventDtos;

public record UserDataEventDto(Guid Id, string Name, string Surname, string Email, DateTimeOffset RegistrationDate, string? GsmNumber, string? City, string? Country, string? Address, string? ZipCode);