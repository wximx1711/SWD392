// Hồ Tuấn Minh | HE191253 | SE1990NJ | SWD392
using System.Text.Json;
public class UpdateCourtProfile(AdminRepository repository, AuditWriter audit, WriteGate gate, IBookingConflictChecker bookings)
{
    public async Task<Court> Execute(int id, CourtUpdate input, int actor)
    {
        using var lease = await gate.Enter();
        await using var transaction = await repository.Begin();
        var court = await repository.Court(id);
        Validation.Court(input.Name, input.Surface, input.Notes);
        Validation.Version(input.Version, court.Version);
        Validation.Text(input.Rationale, "rationale", 1000);
        if (!Catalog.Statuses.Contains(input.Status)) throw ApiException.Field("status", "Trạng thái sân không hợp lệ.");
        if (await repository.CourtNameExists(court.FacilityId, input.Name, id))
            throw ApiException.Field("name", "Tên sân phải duy nhất trong cơ sở này.", 409);
        if (court.Status != input.Status && input.Status != "Active") await bookings.CheckCourtStatus(id);
        var before = JsonSerializer.Serialize(court);
        court.Name = input.Name.Trim(); court.Surface = input.Surface; court.Status = input.Status;
        court.Notes = input.Notes; court.Lighting = input.Lighting; court.UpdatedAt = DateTime.UtcNow; court.Version++;
        audit.Write("Court", "UC54", id, actor, before, court, input.Rationale.Trim());
        await repository.Save();
        await transaction.CommitAsync();
        return court;
    }
}

