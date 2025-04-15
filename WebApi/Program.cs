using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
using WebApi.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(
    options => options.UseSqlite(builder.Configuration["ConnectionStrings:EFCoreConsole"])
);

// Ativa o suporte para Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("/createDB", (Context context) =>
{
    context.Database.EnsureCreated();
});

app.MapGet("/diretores", (Context context) =>
{
    return context.Diretores
        .Include(diretor => diretor.Filmes)
        .ToList();
})
.WithOpenApi();

app.MapGet("/diretores/{id}", (Context context, int id) =>
{
    return context.Diretores
        .Where(diretor => diretor.Id == id)
        .Include(diretor => diretor.Filmes)
        .ToList();
})
.WithOpenApi();

app.MapGet("/filmes/{id}", (Context context, int id) =>
{
    return context.Filmes
        .Where(filme => filme.Id == id)
        .Include(filme => filme.Diretor)
        .ToList();
})
.WithOpenApi();

app.MapGet("/filmes/byName/{titulo}", (
    Context context,
    string titulo) =>
{
    // return context.Filmes
    //     .Where(filme => filme.Titulo.Contains(titulo))
    //     .Include(filme => filme.Diretor)
    //     .ToList();

    return context.Filmes
        .Where(filme =>
                    EF.Functions.Like(filme.Titulo, $"%{titulo}%")
        )
        .Include(filme => filme.Diretor)
        .ToList();
})
.WithOpenApi();

app.MapGet("/filmes", (Context context) =>
{
    return context.Filmes
        .Include(filme => filme.Diretor)
        .OrderByDescending(filme => filme.Ano)
        .ThenByDescending(filme => filme.Titulo)
        .ToList();
})
.WithOpenApi();

app.MapPost("/diretores", (Context context, Diretor diretor) =>
{
    context.Diretores.Add(diretor);
    context.SaveChanges();
})
.WithOpenApi();

app.MapPut("/diretores/{diretorId}", (Context context, int diretorId, Diretor diretorNovo) =>
{
    var diretor = context.Diretores.Find(diretorId);

    if (diretor != null)
    {
        diretor.Name = diretorNovo.Name;
        if (diretorNovo.Filmes.Count > 0)
        {
            diretor.Filmes.Clear();
            foreach (var filme in diretorNovo.Filmes)
            {
                diretor.Filmes.Add(filme);
            }
        }
    }
    context.SaveChanges();
})
.WithOpenApi();

app.MapDelete("/diretores/{diretorId}", (Context context, int diretorId) =>
{
    var diretor = context.Diretores.Find(diretorId);
    
    if (diretor != null)
    {
        context.Remove(diretor);
    }
    context.SaveChanges();
})
.WithOpenApi();

app.Run();