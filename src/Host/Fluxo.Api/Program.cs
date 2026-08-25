using Fluxo.BuildingBlocks.Application;
using Fluxo.Modules.IdentityAccess.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDispatcher();

// One line per module — this is the only place in the codebase allowed to know about
// every Bounded Context at once (constitution.md Artículo IV).
builder.Services.AddHandlersFrom(typeof(IdentityAccessModule).Assembly);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
