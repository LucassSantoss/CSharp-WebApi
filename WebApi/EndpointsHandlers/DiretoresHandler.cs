using WebApi.Domain.Entities;
using WebApi.Repo.Contracts;

namespace WebApi.EndpointHandlers;

public static class DiretoresHandler
{
    public static IResult GetDiretores(IDiretorRepository diretorRepository)
    {
        return Results.Ok(diretorRepository.GetDiretores());
    }

    public static IResult GetDiretorByName(IDiretorRepository diretorRepository, string name)
    {
        return Results.Ok(diretorRepository.GetDiretorByName(name));
    }

    public static IResult GetDiretorById(IDiretorRepository diretorRepository, int id)
    {
        return Results.Ok(diretorRepository.GetDiretorById(id));
    }

    public static IResult AddDiretor(IDiretorRepository diretorRepository, Diretor diretor)
    {
        diretorRepository.Add(diretor);
        diretorRepository.SaveChanges();
        return Results.Created();
    }

    public static IResult UpdateDiretor(IDiretorRepository diretorRepository, Diretor diretorNovo)
    {
        bool result = diretorRepository.Update(diretorNovo);
        if (result) {
            diretorRepository.SaveChanges();
            return Results.Ok();
        };
        return Results.NotFound();
    }

    public static IResult DeleteDiretor(IDiretorRepository diretorRepository, int diretorId)
    {
        bool result = diretorRepository.Delete(diretorId);
        if (result) {
            diretorRepository.SaveChanges();
            return Results.Ok();
        }
        return Results.NotFound($"Diretor com id {diretorId} não encontrado");
    }
}
