public record FacilityView(int Id, string Name, string Address, string CityDistrict, string Phone, string Email,
    TimeOnly Opening, TimeOnly Closing, int SlotMinutes, bool Parking, bool Shower, bool ProShop, bool Wifi,
    decimal? Latitude, decimal? Longitude, string Status, DateTime UpdatedAt, int Version, string? ImageUrl)
{
    public static FacilityView From(Facility f) => new(f.Id, f.Name, f.Address, f.CityDistrict, f.Phone, f.Email, f.Opening,
        f.Closing, f.SlotMinutes, f.Parking, f.Shower, f.ProShop, f.Wifi, f.Latitude, f.Longitude, f.Status, f.UpdatedAt, f.Version, f.ImageUrl);
}
public record CourtView(int Id, int FacilityId, string Name, string Sport, string Surface, bool Indoor, string? Notes,
    bool Lighting, string Status, DateTime UpdatedAt, int Version)
{
    public static CourtView From(Court c) => new(c.Id, c.FacilityId, c.Name, c.Sport, c.Surface, c.Indoor, c.Notes,
        c.Lighting, c.Status, c.UpdatedAt, c.Version);
}
public record PolicyView(int Id, string Name, string Scope, int? FacilityId, string Sport, decimal Regular, decimal Peak,
    decimal Weekend, decimal Discount, TimeOnly PeakStart, TimeOnly PeakEnd, DateOnly EffectiveStart, DateOnly? EffectiveEnd, string Status)
{
    public static PolicyView From(Policy p, BusinessClock clock) => new(p.Id, p.Name, p.Scope, p.FacilityId, p.Sport, p.Regular,
        p.Peak, p.Weekend, p.Discount, p.PeakStart, p.PeakEnd, p.EffectiveStart, p.EffectiveEnd, clock.PolicyStatus(p));
}
