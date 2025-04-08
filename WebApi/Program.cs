using WebApi.DbContexts;

var builder = WebApplication.CreateBuilder(args);

using (var context = new Context())
{
    context.Database.EnsureCreated();
}

// Ativa o suporte para Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/diretor", () =>
{
    var context = new Context();
    return context.Diretores.ToList();
})
.WithOpenApi();

app.Run();