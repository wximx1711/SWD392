// Hồ Tuấn Minh | HE191253 | SE1990NJ | SWD392
using System.Text.Json;
// UC52: identity stays in the route; operating boundaries belong to the facility.
public class UpdateFacilityProfile(AdminRepository repository, AuditWriter audit, WriteGate gate, IBookingConflictChecker bookings)
{
    public async Task<Facility> Execute(int id, FacilityInput input, int actor)
    {
        using var lease = await gate.Enter();
        await using var transaction = await repository.Begin();
        var facility = await repository.Facility(id);
        if (facility.Status != "Active") throw new ApiException(409, "Chỉ được sửa cơ sở đang hoạt động.");
        Validation.Facility(input);
        Validation.Version(input.Version, facility.Version);
        Validation.Text(input.Rationale, "rationale", 1000);
        if (await repository.FacilityNameExists(input.Name, id))
            throw ApiException.Field("name", "Tên cơ sở đã tồn tại trong chuỗi.", 409);
        if (input.Opening > facility.Opening || input.Closing < facility.Closing)
            await bookings.CheckFacilityHours(id, input.Opening, input.Closing);
        var before = JsonSerializer.Serialize(facility);
        facility.Apply(input);
        facility.Version++;
        audit.Write("Facility", "UC52", id, actor, before, facility, input.Rationale!.Trim());
        await repository.Save();
        await transaction.CommitAsync();
        return facility;
    }
}

