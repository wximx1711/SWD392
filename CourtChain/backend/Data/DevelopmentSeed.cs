using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
public static class DevelopmentSeed
{
    public static async Task Initialize(AppDb db, string? password)
    {
        if (await db.Users.AnyAsync() || string.IsNullOrWhiteSpace(password)) return;
        if (password.Length < 10) throw new InvalidOperationException("COURTCHAIN_OWNER_PASSWORD must contain at least 10 characters.");
        var salt = RandomNumberGenerator.GetBytes(16);
        db.Users.Add(new User
        {
            Email = "owner@courtchain.local",
            Role = "CourtOwner",
            Salt = Convert.ToBase64String(salt),
            Hash = Passwords.Hash(password, salt)
        });
        await db.SaveChangesAsync();
    }
}
