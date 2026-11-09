using System.ComponentModel.DataAnnotations;
public record LoginRequest([Required] string Email, [Required] string Password);
// UC51/UC52 share profile fields. Only UC52 consumes Version and Rationale.
public record FacilityInput(string Name, string Address, string CityDistrict, string Phone, string Email,
    TimeOnly Opening, TimeOnly Closing, int SlotMinutes, bool Parking, bool Shower, bool ProShop, bool Wifi,
    decimal? Latitude, decimal? Longitude, string? Rationale = null, int? Version = null, string? ImageUrl = null);
public record CourtInput(int FacilityId, string Name, string Sport, string Surface, bool Indoor, string? Notes, bool Lighting, int Count = 1);
// No Id, FacilityId or Sport: identity and membership cannot be edited by UC54.
public record CourtUpdate(string Name, string Surface, string Status, string? Notes, bool Lighting, int Version, string Rationale);
public record PolicyInput(string Name, string Scope, int? FacilityId, string Sport, decimal Regular, decimal Peak,
    decimal Weekend, decimal Discount, TimeOnly PeakStart, TimeOnly PeakEnd, DateOnly EffectiveStart);
