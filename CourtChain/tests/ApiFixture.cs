using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
public sealed class ApiFixture : WebApplicationFactory<Program>
{
    public string Password { get; } = Guid.NewGuid().ToString("N");
    private readonly string database = Path.Combine(Path.GetTempPath(), "courtchain-test-" + Guid.NewGuid() + ".db");
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureAppConfiguration((_, c) => c.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:CourtChain"] = "Data Source=" + database, ["COURTCHAIN_OWNER_PASSWORD"] = Password }));
    public async Task<HttpClient> Owner()
    {
        var c = CreateClient(); var r = await c.PostAsJsonAsync("/api/login", new { email = "owner@courtchain.local", password = Password }); r.EnsureSuccessStatusCode();
        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (await r.Content.ReadFromJsonAsync<Token>())!.token); return c;
    }
    public static FacilityInput Facility(string? name = null) => new(name ?? "Hòa Lạc " + Guid.NewGuid(), "Đại lộ Thăng Long", "Hà Nội", "0912345678", "a@example.com", new(6, 0), new(22, 0), 60, true, true, false, true, null, null);
    public static PolicyInput Policy(int id) => new("Standard", "Facility", id, "Badminton", 80000, 120000, 140000, 10, new(17, 0), new(22, 0), new BusinessClock().Today);
    public async Task<FacilityView> CreateFacility(HttpClient c) { var r = await c.PostAsJsonAsync("/api/facilities", Facility()); r.EnsureSuccessStatusCode(); return (await r.Content.ReadFromJsonAsync<FacilityView>())!; }
    public async Task<CourtView> CreateCourt(HttpClient c, int id, string name = "Badminton 01") { var r = await c.PostAsJsonAsync("/api/courts", new CourtInput(id, name, "Badminton", "Wood", true, "", true)); r.EnsureSuccessStatusCode(); return (await r.Content.ReadFromJsonAsync<CourtView[]>())![0]; }
    public static CourtUpdate Edit(CourtView c) => new(c.Name, c.Surface, c.Status, c.Notes, c.Lighting, c.Version, "Cập nhật demo");
    private record Token(string token);
}
