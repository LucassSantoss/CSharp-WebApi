using System;
using WebApi.EndpointHandlers;

namespace WebApi.EndpointsExtensions;

public static class EndpointDiretores
{
    public static void DiretoresEnpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/diretores", DiretoresHandler.GetDiretores).WithOpenApi();

        app.MapGet("/diretores/byName/{name}", DiretoresHandler.GetDiretorByName).WithOpenApi();

        app.MapGet("/diretores/{id}", DiretoresHandler.GetDiretorById).WithOpenApi();

        app.MapPost("/diretores", DiretoresHandler.AddDiretor).WithOpenApi();

        app.MapPut("/diretores", DiretoresHandler.UpdateDiretor).WithOpenApi();

        app.MapDelete("/diretores/{diretorId}", DiretoresHandler.DeleteDiretor).WithOpenApi();
    }
}
