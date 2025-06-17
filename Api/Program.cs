using FeriadosNacionaisAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Adiciona o serviços para controller
builder.Services.AddControllers();

// Adiciona o HTTP Client para fazer requisições HTTP
builder.Services.AddHttpClient();

//Ferramenta responsável por procurar os serviços de forma automática
builder.Services.AddEndpointsApiExplorer();

// Adiciona o Swagger para documentação da API
builder.Services.AddSwaggerGen();

// Configura o Entity Framework Core com o banco de dados em memória
builder.Services.AddDbContext<FeriadoDbContext>(options =>
    options.UseSqlite("Data Source=FeriadosNacionais.db"));

var app = builder.Build();

// Um endpoint de teste para saber que está funcionando
// app.MapGet("/", () => "O servidor está no ar!");

// Configura o uso do Swagger apenas em ambiente de desenvolvimento
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
app.Run();