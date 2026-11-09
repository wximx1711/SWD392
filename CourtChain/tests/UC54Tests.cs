using System.Net;
using System.Net.Http.Json;
public class UC54Tests(ApiFixture app) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task UpdatesAndRetainsHistory()
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c); var court = await app.CreateCourt(c, f.Id);
        var r = await c.PutAsJsonAsync("/api/courts/" + court.Id, ApiFixture.Edit(court) with { Name = "Badminton VIP 01", Surface = "Rubber", Status = "Under Maintenance" }); Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        var stored = (await c.GetFromJsonAsync<CourtView>("/api/courts/" + court.Id))!; Assert.Equal("Badminton VIP 01", stored.Name); Assert.Equal("Under Maintenance", stored.Status);
        Assert.Equal(2, (await c.GetFromJsonAsync<System.Text.Json.JsonElement[]>($"/api/audits?entity=Court&entityId={court.Id}"))!.Length);
    }
    [Fact]
    public async Task RejectsDuplicateStatusAndMissing()
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c); var a = await app.CreateCourt(c, f.Id); await app.CreateCourt(c, f.Id, "Other");
        Assert.Equal(HttpStatusCode.Conflict, (await c.PutAsJsonAsync("/api/courts/" + a.Id, ApiFixture.Edit(a) with { Name = "Other" })).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await c.PutAsJsonAsync("/api/courts/" + a.Id, ApiFixture.Edit(a) with { Status = "Broken" })).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await c.PutAsJsonAsync("/api/courts/999999", ApiFixture.Edit(a))).StatusCode);
    }
}
