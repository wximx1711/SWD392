using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api")]
public class SupportController(AdminRepository repository, BusinessClock clock, IBookingConflictChecker bookings) : OwnerController
{
    [HttpGet("options")]
    public object Options() => new
    {
        sports = Catalog.Sports,
        surfaces = Catalog.Surfaces,
        statuses = Catalog.Statuses,
        slotMinutes = Catalog.SlotMinutes,
        today = clock.Today,
        bookingChecksAvailable = bookings.Available
    };
    [HttpGet("audits")]
    public async Task<object> Audits(string? entity, int? entityId)
    {
        var query = repository.Audits;
        if (entity != null) query = query.Where(x => x.Entity == entity);
        if (entityId != null) query = query.Where(x => x.EntityId == entityId);
        return await query.OrderByDescending(x => x.Id).Take(200)
            .Select(x => new { x.Id, x.Entity, x.EntityId, x.Action, x.ActorId, x.At, x.Before, x.After, x.Rationale }).ToListAsync();
    }
}
