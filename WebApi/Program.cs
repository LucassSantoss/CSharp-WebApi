using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
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
        .Include(diretor => diretor.Filmes)
        .FirstOrDefault(diretor => diretor.Id == id);
})
.WithOpenApi();

app.MapGet("/diretores/byName/{name}", (Context context, string name) =>
{
    return context.Diretores
        .Include(diretor => diretor.Filmes)
        .FirstOrDefault(diretor => diretor.Name.Contains(name))
        ?? new Diretor { Id = -1, Name = "Inexistente" };
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

app.MapGet("/filmes", (Context context) =>
{
    return context.Filmes
        .Include(filme => filme.Diretor)
        .OrderByDescending(filme => filme.Ano)
        .ThenByDescending(filme => filme.Titulo)
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

app.MapPatch("/filmesSemExecuteUpdate", (Context context, FilmeUpdate filmeUpdate) =>
{
    var filme = context.Filmes.Find(filmeUpdate.Id);

    if (filme == null) return Results.NotFound($"Filme com id {filmeUpdate.Id} não encontrado");

    filme.Titulo = filmeUpdate.Titulo;
    filme.Ano = filmeUpdate.Ano;

    context.Filmes.Update(filme);
    context.SaveChanges();
    return Results.Ok($"Filme com id {filmeUpdate.Id} atualizado com sucesso");
});

app.MapPatch("/filmes", (Context context, FilmeUpdate filmeUpdate) =>
{
    int affectedRows = context.Filmes
        .Where(filme => filme.Id == filmeUpdate.Id)
        .ExecuteUpdate(setter => setter
            .SetProperty(f => f.Titulo, filmeUpdate.Titulo)
            .SetProperty(f => f.Ano, filmeUpdate.Ano)
        );

    if (affectedRows > 0)
    {
        return Results.Ok(
            $"Você teve um total de {affectedRows} linha(s) afetada(s)"
        );
    } else {
        return Results.NoContent();
    }
});

app.MapDelete("/filmes/{filmeId}", (Context context, int filmeId) =>
{
    var filme = context.Filmes
        .Where(filme => filme.Id == filmeId)
        .ExecuteDelete<Filme>();
    context.SaveChanges();
});

app.Run();