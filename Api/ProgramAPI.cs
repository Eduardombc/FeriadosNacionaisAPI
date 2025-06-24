using FeriadosNacionaisAPI.Data;
using Microsoft.EntityFrameworkCore;
using FeriadosNacionaisAPI.Api.Services;
using FeriadosNacionaisAPI.Api.Services.Interfaces;
using FeriadosNacionaisAPI.Api.Configuration;


var builder = WebApplication.CreateBuilder(args);

//Adiciona o serviços para controller
builder.Services.AddControllers();

//Ferramenta responsável por procurar os serviços de forma automática
builder.Services.AddEndpointsApiExplorer();

// Adiciona o Swagger para documentação da API
builder.Services.AddSwaggerGen();

// Adiciona o serviço de FeriadoService para injeção de dependência
builder.Services.AddHttpClient<IFeriadoService, FeriadoService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["NagerDateApiSettings:BaseUrl"]!);
});

// Configura o Entity Framework Core com o banco de dados em memória
builder.Services.AddDbContext<FeriadosDbContext>(options =>
    options.UseSqlite("Data Source=FeriadosNacionais.db"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Expõe o arquivo JSON do Swagger
    app.UseSwagger();
    // Configura o Swagger UI para ser acessível na raiz da aplicação
    app.UseSwaggerUI();
}

// Configura o redirecionamento de HTTP para HTTPS
app.UseHttpsRedirection();

// Configura o uso de controllers
app.MapControllers();

// Inicia o servidor
await app.RunAsync();