using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
public class AuthorizationTests(ApiFixture app) : IClassFixture<ApiFixture>
{
    [Theory][InlineData("/api/facilities")][InlineData("/api/pricing-policies")][InlineData("/api/audits")] public async Task Anonymous(string path) { using var c = app.CreateClient(); Assert.Equal(HttpStatusCode.Unauthorized, (await c.GetAsync(path)).StatusCode); }
    [Fact]
    public async Task NonOwnerAndLogout()
    {
        using var c = await app.Owner(); using (var scope = app.Services.CreateScope()) { var db = scope.ServiceProvider.GetRequiredService<AppDb>(); var u = new User { Email = "manager@example.com", Role = "FacilityManager" }; db.Users.Add(u); await db.SaveChangesAsync(); var token = scope.ServiceProvider.GetRequiredService<Sessions>().Create(u.Id); using var denied = app.CreateClient(); denied.DefaultRequestHeaders.Authorization = new("Bearer", token); Assert.Equal(HttpStatusCode.Forbidden, (await denied.PostAsJsonAsync("/api/pricing-policies", ApiFixture.Policy(1))).StatusCode); }
        Assert.Equal(HttpStatusCode.NoContent, (await c.PostAsync("/api/logout", null)).StatusCode); Assert.Equal(HttpStatusCode.Unauthorized, (await c.GetAsync("/api/facilities")).StatusCode);
    }
}
