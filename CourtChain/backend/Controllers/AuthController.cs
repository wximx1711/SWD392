using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[ApiController]
[Route("api")]
public class AuthController(AppDb db, Sessions sessions) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest input)
    {
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == input.Email.Trim());
        if (user == null || !Passwords.Verify(user, input.Password)) return Unauthorized(new { error = "Email hoặc mật khẩu không đúng." });
        if (user.Role != "CourtOwner") return StatusCode(403, new { error = "Chỉ chủ sân được truy cập quản trị chuỗi." });
        return Ok(new { token = sessions.Create(user.Id), role = user.Role, email = user.Email });
    }
    [Authorize(Policy = "CourtOwner")]
    [HttpGet("me")] public object Me() => new { email = User.Identity!.Name, role = "CourtOwner" };
    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var header = Request.Headers.Authorization.ToString();
        if (header.StartsWith("Bearer ")) sessions.Remove(header[7..]);
        return NoContent();
    }
}
