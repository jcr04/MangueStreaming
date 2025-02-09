using MangueStreaming.Application.Commands.CriarVideo;
using MangueStreaming.Application.Commands.RetornarVideo;
using MangueStreaming.Application.Interfaces;
using MangueStreaming.Domain.Models;
using MangueStreaming.Domain.Repositories;
using MangueStreaming.Infra.Data;
using MangueStreaming.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Obter a connection string do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

// Configurar o DbContext para usar PostgreSQL
builder.Services.AddDbContext<MangueStreamingDbContext>(options =>
    options.UseNpgsql(connectionString));

// Registrar repositórios e command handlers
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<ICommandHandler<CreateVideoCommand, Guid>, CreateVideoCommandHandler>();

//RetornarVideo
builder.Services.AddScoped<IRetornaVideoRepository, RetornaVideoRepository>();
builder.Services.AddScoped<ICommandHandler<RetornaVideoCommand, VideoUrlDto?>, RetornaVideoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<RetornaTodosVideosCommand, List<VideoUrlDto>>, RetornaTodosVideosCommandHandler>();


// Adicionar controllers
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.Run();
