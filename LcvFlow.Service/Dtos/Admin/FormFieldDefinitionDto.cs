namespace LcvFlow.Service.Dtos.Admin;

public class FormFieldDefinitionDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Label { get; set; } = string.Empty; // Soru: "Alerji Durumu"
    public string FieldType { get; set; } = "Text"; // Text, Number, Boolean
    public bool IsRequired { get; set; } = false;
}