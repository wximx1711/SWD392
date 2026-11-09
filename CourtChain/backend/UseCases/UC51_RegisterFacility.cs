// Hồ Tuấn Minh | HE191253 | SE1990NJ | SWD392
// UC51: validate -> create Active facility and operating template -> audit -> commit.
public class RegisterFacility(AdminRepository repository, AuditWriter audit, WriteGate gate)
{
    public async Task<Facility> Execute(FacilityInput input, int actor)
    {
        Validation.Facility(input);
        using var lease = await gate.Enter();
        await using var transaction = await repository.Begin();
        if (await repository.FacilityNameExists(input.Name))
            throw ApiException.Field("name", "Tên cơ sở đã tồn tại trong chuỗi.", 409);
        var facility = new Facility();
        facility.Apply(input);
        repository.Add(facility);
        await repository.Save();
        audit.Write("Facility", "UC51", facility.Id, actor, null, facility, "Đăng ký cơ sở mới.");
        await repository.Save();
        await transaction.CommitAsync();
        return facility;
    }
}

