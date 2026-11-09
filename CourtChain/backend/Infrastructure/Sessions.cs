using System.Collections.Concurrent;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public sealed class Sessions
{
    private readonly ConcurrentDictionary<string, Session> tokens = new();
    public string Create(int userId) { var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32)); tokens[token] = new(userId, DateTime.UtcNow); return token; }
    public int? Resolve(string token)
    {
        if (!tokens.TryGetValue(token, out var s)) return null;
        if (DateTime.UtcNow - s.LastActivity > TimeSpan.FromMinutes(30)) { Remove(token); return null; }
        tokens[token] = s with { LastActivity = DateTime.UtcNow };
        return s.UserId;
    }
    public void Remove(string token) => tokens.TryRemove(token, out _);
    private record Session(int UserId, DateTime LastActivity);
}
public class SessionAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
    UrlEncoder encoder, Sessions sessions, AppDb db) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var header = Request.Headers.Authorization.ToString();
        var id = header.StartsWith("Bearer ") ? sessions.Resolve(header[7..]) : null;
        if (id == null) return AuthenticateResult.NoResult();
        var user = await db.Users.FindAsync(id.Value);
        if (user == null) return AuthenticateResult.Fail("Invalid user");
        var identity = new ClaimsIdentity([new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(ClaimTypes.Name, user.Email), new(ClaimTypes.Role, user.Role)], Scheme.Name);
        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}
public static class Passwords
{
    public static string Hash(string password, byte[] salt) =>
        Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(password, salt, 210000, HashAlgorithmName.SHA256, 32));
    public static bool Verify(User user, string password) =>
        CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(Hash(password, Convert.FromBase64String(user.Salt))), Convert.FromBase64String(user.Hash));
}
