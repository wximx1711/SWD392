using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
[ApiController]
[Authorize(Policy = "CourtOwner")]
public abstract class OwnerController : ControllerBase
{
    protected int ActorId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
