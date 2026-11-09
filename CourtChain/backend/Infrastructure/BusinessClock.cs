public class BusinessClock
{
    public virtual DateTime UtcNow => DateTime.UtcNow;
    public virtual DateOnly Today => DateOnly.FromDateTime(UtcNow.AddHours(7));
    public string PolicyStatus(Policy p) => p.EffectiveStart > Today ? "Scheduled"
        : p.EffectiveEnd.HasValue && p.EffectiveEnd.Value <= Today ? "Expired" : "Active";
}
