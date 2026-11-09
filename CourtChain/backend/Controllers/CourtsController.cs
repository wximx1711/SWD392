using Microsoft.AspNetCore.Mvc;
[Route("api/courts")]
public class CourtsController(AdminRepository repository, RegisterCourt register, UpdateCourtProfile update) : OwnerController
{
    [HttpGet("{id:int}")] public async Task<CourtView> Detail(int id) => CourtView.From(await repository.Court(id));
    [HttpPost]
    public async Task<IActionResult> Create(CourtInput input)
    {
        var courts = await register.Execute(input, ActorId);
        return Created("/api/courts/" + courts[0].Id, courts.Select(CourtView.From));
    }
    [HttpPut("{id:int}")] public async Task<CourtView> Update(int id, CourtUpdate input) => CourtView.From(await update.Execute(id, input, ActorId));
}
