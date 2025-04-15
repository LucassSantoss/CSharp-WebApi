using System;

namespace WebApi.Models;

public record FilmeUpdate(
    int Id,
    string Titulo,
    int Ano
) {}
