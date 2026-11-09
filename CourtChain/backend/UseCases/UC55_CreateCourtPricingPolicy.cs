// Hồ Tuấn Minh | HE191253 | SE1990NJ | SWD392
using Microsoft.EntityFrameworkCore;
// Scope+sport covers three slot types in one immutable version. All create intervals are open-ended.
public class CreateCourtPricingPolicy(AdminRepository repository, AuditWriter audit, WriteGate gate, BusinessClock clock)
{
    public async Task<Policy> Execute(PolicyInput input, int actor)
    {
        Validation.Policy(input, clock.Today);
        using var lease = await gate.Enter();
        await using var transaction = await repository.Begin();
        if (input.Scope == "Facility") await repository.Facility(input.FacilityId!.Value);
        else if (!await repository.Facilities.AnyAsync()) throw ApiException.Field("scope", "Hãy đăng ký ít nhất một cơ sở trước.");
        // Sport membership comes from master configuration, not from existing courts.
        if (await repository.Policies.AnyAsync(x => x.Scope == input.Scope && x.FacilityId == input.FacilityId
            && x.Sport == input.Sport && (x.EffectiveEnd == null || x.EffectiveEnd > input.EffectiveStart)))
            throw ApiException.Field("effectiveStart", "Khoảng hiệu lực trùng chính sách cùng phạm vi và môn thể thao (BR244).", 409);
        var policy = new Policy
        {
            Name = input.Name.Trim(),
            Scope = input.Scope,
            FacilityId = input.FacilityId,
            Sport = input.Sport,
            Regular = input.Regular,
            Peak = input.Peak,
            Weekend = input.Weekend,
            Discount = input.Discount,
            PeakStart = input.PeakStart,
            PeakEnd = input.PeakEnd,
            EffectiveStart = input.EffectiveStart
        };
        policy.Status = clock.PolicyStatus(policy);
        repository.Add(policy);
        await repository.Save();
        audit.Write("Policy", "UC55", policy.Id, actor, null, policy, "Công bố phiên bản chính sách giá mới.");
        await repository.Save();
        await transaction.CommitAsync();
        return policy;
    }
}

