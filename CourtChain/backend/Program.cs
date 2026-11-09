using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddDbContext<AppDb>(o => o.UseSqlite(builder.Configuration.GetConnectionString("CourtChain") ?? "Data Source=courtchain.db"));
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow);
builder.Services.AddOpenApi();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173").AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddSingleton<Sessions>();
builder.Services.AddSingleton<WriteGate>();
builder.Services.AddSingleton<BusinessClock>();
builder.Services.AddAuthentication("LocalSession").AddScheme<AuthenticationSchemeOptions, SessionAuthentication>("LocalSession", null);
builder.Services.AddAuthorization(o => o.AddPolicy("CourtOwner", p => p.RequireAuthenticatedUser().RequireRole("CourtOwner")));
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<AuditWriter>();
builder.Services.AddScoped<RegisterFacility>();
builder.Services.AddScoped<UpdateFacilityProfile>();
builder.Services.AddScoped<RegisterCourt>();
builder.Services.AddScoped<UpdateCourtProfile>();
builder.Services.AddScoped<CreateCourtPricingPolicy>();
builder.Services.AddScoped<IBookingConflictChecker, BookingModuleNotInstalled>();
var app = builder.Build();
app.UseMiddleware<ApiErrors>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapOpenApi();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    await db.Database.MigrateAsync();
    await DevelopmentSeed.Initialize(db, builder.Configuration["COURTCHAIN_OWNER_PASSWORD"]);
}
app.Run();
public partial class Program;
