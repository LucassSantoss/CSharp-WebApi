using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
using WebApi.EndpointsExtensions;
using WebApi.Repo;
using WebApi.Repo.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Ativa o suporte para Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(
    options => options.UseSqlite(builder.Configuration["ConnectionStrings:EFCoreConsole"])
);

builder.Services.AddScoped<IDiretorRepository, DiretorRepository>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.AllowTrailingCommas = true;
    // Evita loop infinito por conta do Diretor na classe filme e vice versa
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.DiretoresEnpoints();

app.FilmesEndpoints();

app.Run();