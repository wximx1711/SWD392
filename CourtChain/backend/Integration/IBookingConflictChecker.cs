// Integration seam for BR223/BR234. Does not invent booking records.
public interface IBookingConflictChecker
{
    bool Available { get; }
    Task CheckFacilityHours(int facilityId, TimeOnly opening, TimeOnly closing);
    Task CheckCourtStatus(int courtId);
}
public class BookingModuleNotInstalled : IBookingConflictChecker
{
    public bool Available => false;
    public Task CheckFacilityHours(int facilityId, TimeOnly opening, TimeOnly closing) => Task.CompletedTask;
    public Task CheckCourtStatus(int courtId) => Task.CompletedTask;
}
