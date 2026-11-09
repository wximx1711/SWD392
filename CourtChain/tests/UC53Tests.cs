using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
public class UC53Tests(ApiFixture app) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task CourtMembershipAndAtomicBatch()
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c); var court = await app.CreateCourt(c, f.Id, "Batch 02"); Assert.Equal(f.Id, court.FacilityId); Assert.Equal("Active", court.Status);
        var input = new CourtInput(f.Id, "Batch", "Badminton", "Wood", true, "", true, 3);
        Assert.Equal(HttpStatusCode.Conflict, (await c.PostAsJsonAsync("/api/courts", input)).StatusCode); Assert.Single((await c.GetFromJsonAsync<CourtView[]>($"/api/facilities/{f.Id}/courts"))!);
        Assert.Equal(HttpStatusCode.Created, (await c.PostAsJsonAsync("/api/courts", input with { Name = "Group" })).StatusCode); Assert.Equal(4, (await c.GetFromJsonAsync<CourtView[]>($"/api/facilities/{f.Id}/courts"))!.Length);
    }
    [Theory]
    [InlineData("sport", 400)]
    [InlineData("missing", 404)]
    [InlineData("inactive", 409)]
    public async Task InvalidParentOrSport(string kind, int status)
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c);
        if (kind == "inactive") { using var scope = app.Services.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<AppDb>(); (await db.Facilities.FindAsync(f.Id))!.Status = "Inactive"; await db.SaveChangesAsync(); }
        Assert.Equal(status, (int)(await c.PostAsJsonAsync("/api/courts", new CourtInput(kind == "missing" ? 999999 : f.Id, "A", kind == "sport" ? "Football" : "Badminton", "Wood", true, "", true))).StatusCode);
    }
}
