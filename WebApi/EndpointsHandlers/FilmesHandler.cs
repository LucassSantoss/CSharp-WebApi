using System;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
using WebApi.Domain.Entities;
using WebApi.Models;

namespace WebApi.EndpointHandlers;

public static class FilmesHandler
{
    public static IResult GetFilmes(Context context)
    {
        return Results.Ok(context.Filmes
            .Include(filme => filme.Diretor)
            .OrderByDescending(filme => filme.Ano)
            .ThenByDescending(filme => filme.Titulo)
            .ToList());
    }

    public static IResult GetFilmeById(Context context, int id)
    {
        return Results.Ok(context.Filmes
            .Where(filme => filme.Id == id)
            .Include(filme => filme.Diretor)
            .ToList());
    }

    public static IResult GetFilmeByName(Context context, string titulo)
    {

        return Results.Ok(context.Filmes
            .Where(filme => EF.Functions.Like(filme.Titulo, $"%{titulo}%"))
            //.Where(filme => filme.Titulo.ToLower().Contains(titulo.ToLower()))
            .Include(filme => filme.Diretor)
            .ToList());
    }

    public static IResult UpdateFilme(Context context, FilmeUpdate filmeUpdate)
    {
        int affectedRows = context.Filmes
            .Where(filme => filme.Id == filmeUpdate.Id)
            .ExecuteUpdate(setter => setter
                .SetProperty(f => f.Titulo, filmeUpdate.Titulo)
                .SetProperty(f => f.Ano, filmeUpdate.Ano)
            );
        if (affectedRows == 0) return Results.NotFound();
        return Results.Ok($"Você teve um total de {affectedRows} linha(s) afetada(s)");
    }

    public static IResult DeleteFilme(Context context, int filmeId)
    {
        return Results.Ok(context.Filmes
            .Where(filme => filme.Id == filmeId)
            .ExecuteDelete<Filme>());
    }
}
