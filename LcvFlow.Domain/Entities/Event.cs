using LcvFlow.Domain.Common;

namespace LcvFlow.Domain.Entities;

public class Event : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public ICollection<Guest> Guests { get; set; } = [];
}