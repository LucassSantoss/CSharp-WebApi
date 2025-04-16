using System;
using WebApi.Domain.Entities;

namespace WebApi.Repo.Contracts;

public interface IDiretorRepository
{

    List<Diretor> GetDiretores();
    Diretor GetDiretorByName(string name);
    Diretor GetDiretorById(int id);
    void Add(Diretor diretor);
    bool Update(Diretor diretor);
    bool Delete(int diretorId);
    bool SaveChanges();
}
