using System;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
using WebApi.Domain.Entities;
using WebApi.Repo.Contracts;

namespace WebApi.Repo;

public class DiretorRepository(Context _context) : IDiretorRepository
{
    public Context Context { get; } = _context;

    public List<Diretor> GetDiretores()
    {
        return Context.Diretores
            .Include(diretor => diretor.Filmes)
            .ToList();
    }

    public Diretor GetDiretorByName(string name)
    {
        return Context.Diretores
            .Include(diretor => diretor.Filmes)
            .FirstOrDefault(diretor => diretor.Name.Contains(name))
            ?? new Diretor { Id = -1, Name = "Inexistente" };
    }

    public Diretor GetDiretorById(int id)
    {
        return Context.Diretores
            .Include(diretor => diretor.Filmes)
            .FirstOrDefault(diretor => diretor.Id == id)
            ?? new Diretor { Id = -1, Name = "Inexistente" };
    }

    public void Add(Diretor diretor)
    {
        Context.Diretores.Add(diretor);
    }

    public bool Update(Diretor diretorNovo)
    {
        var diretor = Context.Diretores.Find(diretorNovo.Id);

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
            return true;
        }
        return false;
    }

    public bool Delete(int diretorId)
    {
        var diretor = Context.Diretores.Find(diretorId);

        if (diretor != null)
        {
            Context.Remove(diretor);
            Context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool SaveChanges()
    {
        return Context.SaveChanges() > 0;
    }
}
