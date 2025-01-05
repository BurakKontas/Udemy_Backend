using Udemy.Common.Base;
using Udemy.Common.Events.EventDtos;

namespace Udemy.Payment.Domain.Entities;

public class UserDataEntity : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? GsmNumber { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }

    public static UserDataEntity ToEntity(UserDataEventDto dto)
    {
        return new UserDataEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            Surname = dto.Surname,
            Email = dto.Email,
            RegistrationDate = dto.RegistrationDate,
            GsmNumber = dto.GsmNumber,
            City = dto.City,
            Country = dto.Country,
            Address = dto.Address,
            ZipCode = dto.ZipCode
        };
    }

    public UserDataEventDto ToDto()
    {
        return new UserDataEventDto(Id, Name, Surname, Email, RegistrationDate, GsmNumber, City, Country, Address, ZipCode);
    }

}