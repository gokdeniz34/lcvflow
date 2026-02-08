using LcvFlow.Domain.Common;

namespace LcvFlow.Domain.Entities;

public class SystemSetting : BaseEntity
{
    public string SettingKey { get; set; } = null!; // Örn: "AllowTestData"
    public string SettingValue { get; set; } = null!; // Örn: "true"
    public string Description { get; set; } = string.Empty;
}