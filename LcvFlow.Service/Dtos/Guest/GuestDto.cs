namespace LcvFlow.Service.Dtos.Guest;

public record GuestDto(int Id, string FirstName, string LastName, string FullName, string? Email, string? EventName);