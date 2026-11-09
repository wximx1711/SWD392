using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/facilities")]
public class FacilitiesController(AdminRepository repository, RegisterFacility register, UpdateFacilityProfile update) : OwnerController
{
    [HttpGet] public async Task<IEnumerable<FacilityView>> List() => (await repository.Facilities.AsNoTracking().OrderBy(x => x.Name).ToListAsync()).Select(FacilityView.From);
    [HttpGet("{id:int}")] public async Task<FacilityView> Detail(int id) => FacilityView.From(await repository.Facility(id));
    [HttpPost]
    public async Task<IActionResult> Create(FacilityInput input)
    {
        var facility = await register.Execute(input, ActorId);
        return CreatedAtAction(nameof(Detail), new { id = facility.Id }, FacilityView.From(facility));
    }
    [HttpPut("{id:int}")] public async Task<FacilityView> Update(int id, FacilityInput input) => FacilityView.From(await update.Execute(id, input, ActorId));
    [HttpGet("{id:int}/courts")]
    public async Task<IEnumerable<CourtView>> Courts(int id)
    {
        await repository.Facility(id);
        return (await repository.Courts.AsNoTracking().Where(x => x.FacilityId == id).OrderBy(x => x.Name).ToListAsync()).Select(CourtView.From);
    }
    [HttpGet("{id:int}/schedule")]
    public async Task<object> Schedule(int id)
    {
        var f = await repository.Facility(id);
        var slots = new List<object>();
        for (var start = f.Opening.ToTimeSpan(); start + TimeSpan.FromMinutes(f.SlotMinutes) <= f.Closing.ToTimeSpan(); start += TimeSpan.FromMinutes(f.SlotMinutes))
            slots.Add(new { start = TimeOnly.FromTimeSpan(start), end = TimeOnly.FromTimeSpan(start + TimeSpan.FromMinutes(f.SlotMinutes)) });
        return new { facilityId = id, f.Version, f.Opening, f.Closing, f.SlotMinutes, slots };
    }
}
