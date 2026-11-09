using System.Net;
using System.Net.Http.Json;
public class UC51Tests(ApiFixture app) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task CreatePersistAuditScheduleAndDuplicate()
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c); Assert.Equal("Active", f.Status);
        Assert.Equal(f.Name, (await c.GetFromJsonAsync<FacilityView>("/api/facilities/" + f.Id))!.Name);
        Assert.Equal(HttpStatusCode.OK, (await c.GetAsync($"/api/facilities/{f.Id}/schedule")).StatusCode);
        var audits = await c.GetFromJsonAsync<System.Text.Json.JsonElement[]>($"/api/audits?entity=Facility&entityId={f.Id}"); Assert.Single(audits!);
        Assert.Equal(HttpStatusCode.Conflict, (await c.PostAsJsonAsync("/api/facilities", ApiFixture.Facility(" " + f.Name.ToUpperInvariant() + " "))).StatusCode);
    }
    [Theory]
    [InlineData("email")]
    [InlineData("phone")]
    [InlineData("hours")]
    [InlineData("short")]
    [InlineData("slot")]
    public async Task InvalidFields(string field) { using var c = await app.Owner(); var f = ApiFixture.Facility(); f = field switch { "email" => f with { Email = "bad" }, "phone" => f with { Phone = "bad" }, "hours" => f with { Closing = new(5, 0) }, "short" => f with { Closing = new(9, 0) }, _ => f with { SlotMinutes = 0 } }; Assert.Equal(HttpStatusCode.BadRequest, (await c.PostAsJsonAsync("/api/facilities", f)).StatusCode); }
}
