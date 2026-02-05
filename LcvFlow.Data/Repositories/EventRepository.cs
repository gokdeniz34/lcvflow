using LcvFlow.Data.Context;
using LcvFlow.Data.Repositories;
using LcvFlow.Domain;
using LcvFlow.Domain.Entities;

namespace LcvFlow.Data;

public class EventRepository : EfRepository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context)
    {
    }
}
