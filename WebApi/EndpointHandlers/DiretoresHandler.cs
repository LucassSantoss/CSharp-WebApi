using System;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
using WebApi.Entities;

namespace WebApi.EndpointHandlers;

public static class DiretoresHandler
{
    public static IResult GetDiretores(Context context)
    {
        return Results.Ok(context.Diretores
            .Include(diretor => diretor.Filmes)
            .ToList());
    }

    public static IResult GetDiretorByName(Context context, string name)
    {
        return Results.Ok(context.Diretores
            .Include(diretor => diretor.Filmes)
            .FirstOrDefault(diretor => diretor.Name.Contains(name))
            ?? new Diretor { Id = -1, Name = "Inexistente" });
    }

    public static IResult GetDiretorById(Context context, int id)
    {
        return Results.Ok(context.Diretores
            .Include(diretor => diretor.Filmes)
            .FirstOrDefault(diretor => diretor.Id == id)
            ?? new Diretor { Id = -1, Name = "Inexistente" });
    }

    public static IResult AddDiretor(Context context, Diretor diretor)
    {
        context.Diretores.Add(diretor);
        context.SaveChanges();
        return Results.Created();
    }

    public static IResult UpdateDiretor(Context context, int diretorId, Diretor diretorNovo)
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
            context.SaveChanges();
            return Results.Ok(diretor);
        }
        return Results.NotFound();
    }

    public static IResult DeleteDiretor(Context context, int diretorId)
    {
        var diretor = context.Diretores.Find(diretorId);

        if (diretor != null)
        {
            context.Remove(diretor);
            context.SaveChanges();
            return Results.Ok();
        }
        return Results.NotFound($"Diretor com id {diretorId} não encontrado");
    }
}
