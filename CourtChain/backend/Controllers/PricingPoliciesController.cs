using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/pricing-policies")]
public class PricingPoliciesController(AdminRepository repository, CreateCourtPricingPolicy create, BusinessClock clock) : OwnerController
{
    [HttpGet] public async Task<IEnumerable<PolicyView>> List() => (await repository.Policies.AsNoTracking().OrderByDescending(x => x.EffectiveStart).ToListAsync()).Select(p => PolicyView.From(p, clock));
    [HttpPost]
    public async Task<IActionResult> Create(PolicyInput input)
    {
        var policy = await create.Execute(input, ActorId);
        return Created("/api/pricing-policies/" + policy.Id, PolicyView.From(policy, clock));
    }
}
