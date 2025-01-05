namespace Udemy.Common.Events.EventDtos;

public record UserDataEventDto(Guid Id, string Name, string Surname, string Email, DateTime RegistrationDate, string? GsmNumber, string? City, string? Country, string? Address, string? ZipCode);