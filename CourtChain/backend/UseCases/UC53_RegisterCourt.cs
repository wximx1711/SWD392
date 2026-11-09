// Hồ Tuấn Minh | HE191253 | SE1990NJ | SWD392
// UC53: includes atomic batch alternative. Count > 1 names: "{prefix} 01", "02", ...
public class RegisterCourt(AdminRepository repository, AuditWriter audit, WriteGate gate)
{
    public async Task<List<Court>> Execute(CourtInput input, int actor)
    {
        Validation.Court(input.Name, input.Surface, input.Notes);
        if (!Catalog.Sports.Contains(input.Sport)) throw ApiException.Field("sport", "Môn thể thao không được hỗ trợ.");
        if (input.Count < 1 || input.Count > 50) throw ApiException.Field("count", "Số sân trong một lần đăng ký từ 1–50.");
        using var lease = await gate.Enter();
        await using var transaction = await repository.Begin();
        var facility = await repository.Facility(input.FacilityId);
        if (facility.Status != "Active") throw new ApiException(409, "Chỉ được thêm sân vào cơ sở đang hoạt động.");
        var names = Enumerable.Range(1, input.Count).Select(i => input.Count == 1 ? input.Name.Trim() : $"{input.Name.Trim()} {i:00}").ToList();
        foreach (var name in names)
            if (await repository.CourtNameExists(input.FacilityId, name))
                throw ApiException.Field("name", $"Tên sân '{name}' đã tồn tại trong cơ sở này. Chưa tạo sân nào.", 409);
        var courts = names.Select(name =>
        {
            var court = new Court { FacilityId = input.FacilityId, Sport = input.Sport };
            court.Apply(input with { Name = name });
            repository.Add(court);
            return court;
        }).ToList();
        await repository.Save();
        foreach (var court in courts)
            audit.Write("Court", "UC53", court.Id, actor, null, court, "Đăng ký sân; sử dụng lịch hoạt động của cơ sở.");
        await repository.Save();
        await transaction.CommitAsync();
        return courts;
    }
}

