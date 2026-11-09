using System.Net;
using System.Net.Http.Json;
public class UC55Tests(ApiFixture app) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task ActivePolicyAndAuditAndOverlap()
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c); var p = ApiFixture.Policy(f.Id); var r = await c.PostAsJsonAsync("/api/pricing-policies", p); Assert.Equal(HttpStatusCode.Created, r.StatusCode); var stored = (await r.Content.ReadFromJsonAsync<PolicyView>())!; Assert.Equal("Active", stored.Status); Assert.Equal(80000m, stored.Regular);
        Assert.Single((await c.GetFromJsonAsync<System.Text.Json.JsonElement[]>($"/api/audits?entity=Policy&entityId={stored.Id}"))!);
        Assert.Equal(HttpStatusCode.Conflict, (await c.PostAsJsonAsync("/api/pricing-policies", p with { EffectiveStart = p.EffectiveStart.AddDays(2) })).StatusCode);
    }
    [Theory]
    [InlineData("zero")]
    [InlineData("negative")]
    [InlineData("peak")]
    [InlineData("past")]
    [InlineData("discount")]
    public async Task InvalidPolicy(string kind) { using var c = await app.Owner(); var f = await app.CreateFacility(c); var p = ApiFixture.Policy(f.Id); p = kind switch { "zero" => p with { Regular = 0 }, "negative" => p with { Weekend = -1 }, "peak" => p with { Peak = 90000 }, "past" => p with { EffectiveStart = p.EffectiveStart.AddDays(-1) }, _ => p with { Discount = 101 } }; Assert.Equal(HttpStatusCode.BadRequest, (await c.PostAsJsonAsync("/api/pricing-policies", p)).StatusCode); }
    [Fact] public async Task ChainScheduledPolicy() { using var c = await app.Owner(); var f = await app.CreateFacility(c); var r = await c.PostAsJsonAsync("/api/pricing-policies", ApiFixture.Policy(f.Id) with { Scope = "Chain", FacilityId = null, Sport = "Pickleball", EffectiveStart = new BusinessClock().Today.AddDays(1) }); Assert.Equal(HttpStatusCode.Created, r.StatusCode); var p = (await r.Content.ReadFromJsonAsync<PolicyView>())!; Assert.Null(p.FacilityId); Assert.Equal("Scheduled", p.Status); }
    [Fact] public void VietnamDateStatus() { var p = new Policy { EffectiveStart = new(2026, 11, 10), Status = "Scheduled" }; Assert.Equal("Active", new FixedClock().PolicyStatus(p)); Assert.Equal("Scheduled", p.Status); }
    private class FixedClock : BusinessClock { public override DateTime UtcNow => new(2026, 11, 9, 18, 0, 0, DateTimeKind.Utc); }
}
