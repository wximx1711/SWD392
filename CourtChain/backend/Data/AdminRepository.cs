using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
public class AdminRepository(AppDb db)
{
    public IQueryable<Facility> Facilities => db.Facilities;
    public IQueryable<Court> Courts => db.Courts;
    public IQueryable<Policy> Policies => db.Policies;
    public IQueryable<Audit> Audits => db.Audits.AsNoTracking();
    public async Task<Facility> Facility(int id) => await db.Facilities.FindAsync(id) ?? throw new ApiException(404, "Không tìm thấy cơ sở.");
    public async Task<Court> Court(int id) => await db.Courts.FindAsync(id) ?? throw new ApiException(404, "Không tìm thấy sân.");
    public void Add<T>(T entity) where T : class => db.Add(entity);
    public Task<int> Save() => db.SaveChangesAsync();
    public Task<IDbContextTransaction> Begin() => db.Database.BeginTransactionAsync();
    public async Task<bool> FacilityNameExists(string name, int? except = null) =>
        (await Facilities.Where(x => x.Id != except).Select(x => x.Name).ToListAsync()).Any(n => Catalog.Normalize(n) == Catalog.Normalize(name));
    public async Task<bool> CourtNameExists(int facilityId, string name, int? except = null) =>
        (await Courts.Where(x => x.FacilityId == facilityId && x.Id != except).Select(x => x.Name).ToListAsync()).Any(n => Catalog.Normalize(n) == Catalog.Normalize(name));
}
