using LcvFlow.Domain.Entities;
using LcvFlow.Service.Dtos.Admin;
using LcvFlow.Service.Dtos.Guest;

namespace LcvFlow.Service.Interfaces;

public interface IExcelService
{
    // Dinamik kolonları da içeren Excel'i parse eder
    Task<List<Guest>> ParseGuestExcelAsync(Stream fileStream, Event ev);
    List<Guest> ParseGuestExcelSync(Stream fileStream, Event ev);

    // Adminin indireceği o meşhur "Yönergeli Şablon"u üretir
    Task<byte[]> GenerateTemplateWithInstructionsAsync(Event ev);
}