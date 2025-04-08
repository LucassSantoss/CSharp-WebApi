using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DbContexts;

public class Context : DbContext
{
    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Diretor> Diretores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("../../../EFCoreConsole.db");
}

