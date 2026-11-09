using System.Net;
using System.Net.Http.Json;
public class UC52Tests(ApiFixture app) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task UpdateAndStaleVersion()
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c); var input = ApiFixture.Facility(f.Name) with { Phone = "0987654321", Closing = new(21, 0), Version = f.Version, Rationale = "Đổi giờ" };
        var r = await c.PutAsJsonAsync("/api/facilities/" + f.Id, input); Assert.Equal(HttpStatusCode.OK, r.StatusCode); var result = (await r.Content.ReadFromJsonAsync<FacilityView>())!; Assert.Equal(f.Id, result.Id); Assert.Equal(input.Phone, result.Phone); Assert.Equal(f.Version + 1, result.Version);
        Assert.Equal(HttpStatusCode.Conflict, (await c.PutAsJsonAsync("/api/facilities/" + f.Id, input)).StatusCode);
    }
    [Fact]
    public async Task MissingInvalidAndImmutableIdentity()
    {
        using var c = await app.Owner(); var f = await app.CreateFacility(c);
        Assert.Equal(HttpStatusCode.NotFound, (await c.PutAsJsonAsync("/api/facilities/999999", ApiFixture.Facility())).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await c.PutAsJsonAsync("/api/facilities/" + f.Id, ApiFixture.Facility(f.Name) with { Email = "bad" })).StatusCode);
        var json = System.Text.Json.JsonSerializer.SerializeToNode(ApiFixture.Facility(f.Name), new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web))!; json["id"] = 999;
        Assert.Equal(HttpStatusCode.BadRequest, (await c.PutAsJsonAsync("/api/facilities/" + f.Id, json)).StatusCode);
    }
}
