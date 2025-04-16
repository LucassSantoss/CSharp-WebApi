using System;
using WebApi.EndpointHandlers;

namespace WebApi.EndpointsExtensions;

public static class EndpointFilmes
{
    public static void FilmesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/filmes", FilmesHandler.GetFilmes).WithOpenApi();

        app.MapGet("/filmes/{id}", FilmesHandler.GetFilmeById).WithOpenApi();

        app.MapGet("/filmes/byName/{titulo}", FilmesHandler.GetFilmeByName).WithOpenApi();

        app.MapPatch("/filmes", FilmesHandler.UpdateFilme).WithOpenApi();

        app.MapDelete("/filmes/{filmeId}", FilmesHandler.DeleteFilme).WithOpenApi();
    }
}
