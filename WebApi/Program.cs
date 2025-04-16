using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
using WebApi.EndpointHandlers;
using WebApi.Entities;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Ativa o suporte para Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(
    options => options.UseSqlite(builder.Configuration["ConnectionStrings:EFCoreConsole"])
);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.AllowTrailingCommas = true;
    // Evita loop infinito por conta do Diretor na classe filme e vice versa
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapGet("/createDB", (Context context) =>
// {
//     context.Database.EnsureCreated();
// });

app.MapGet("/diretores", DiretoresHandler.GetDiretores).WithOpenApi();

app.MapGet("/diretores/byName/{name}", DiretoresHandler.GetDiretorByName).WithOpenApi();

app.MapGet("/diretores/{id}", DiretoresHandler.GetDiretorById).WithOpenApi();

app.MapPost("/diretores", DiretoresHandler.AddDiretor).WithOpenApi();

app.MapPut("/diretores/{diretorId}", DiretoresHandler.UpdateDiretor).WithOpenApi();

app.MapDelete("/diretores/{diretorId}", DiretoresHandler.DeleteDiretor).WithOpenApi();

app.MapGet("/filmes", FilmesHandler.GetFilmes).WithOpenApi();

app.MapGet("/filmes/{id}", FilmesHandler.GetFilmeById).WithOpenApi();

app.MapGet("/filmes/byName/{titulo}", FilmesHandler.GetFilmeByName).WithOpenApi();

app.MapPatch("/filmes", FilmesHandler.UpdateFilme).WithOpenApi();

app.MapDelete("/filmes/{filmeId}", FilmesHandler.DeleteFilme).WithOpenApi();

app.Run();