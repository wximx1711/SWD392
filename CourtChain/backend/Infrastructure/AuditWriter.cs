using System.Text.Json;
public class AuditWriter(AdminRepository repository)
{
    public void Write(string entity, string action, int id, int actor, string? before, object after, string rationale) =>
        repository.Add(new Audit
        {
            Entity = entity,
            Action = action,
            EntityId = id,
            ActorId = actor,
            Before = before,
            After = JsonSerializer.Serialize(after),
            Rationale = rationale
        });
}
