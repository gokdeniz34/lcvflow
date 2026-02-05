namespace LcvFlow.Service.Dtos.Guest;

public record GuestListDto(int Id, string FullName, string? PhoneNumber, bool? IsAttending);