using LcvFlow.Data.Context;
using LcvFlow.Domain.Entities;
using LcvFlow.Domain.Interfaces;

namespace LcvFlow.Data.Repositories;

public class EventRepository : EfRepository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context)
    {
    }
}
