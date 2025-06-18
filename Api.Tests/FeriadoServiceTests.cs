using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using FeriadosNacionaisAPI.Api.Services;
using FeriadosNacionaisAPI.Data;
using FeriadosNacionaisAPI.Api.Models;

namespace Api.Tests
{
    public class FeriadoServiceTests
    {
        private const string NagerDateApiBaseUrl = "https://api.nagerdate.com/v3/";

        [Fact]
        public async Task ObterFeriadosAsync_QuandoCacheExiste_RetornaFeriadosDoBancoDeDados()
        {
            // criação do cenário de teste
            var feriadosFalsos = new List<Feriado>() 
            {
                // CORREÇÃO: Dentro do inicializador, usamos apenas o nome da propriedade, sem "Feriado."
                new Feriado { Id = 1, Name = "Feriado de Teste 1", Date = new DateOnly(2025, 1, 1) },
                new Feriado { Id = 2, Name = "Feriado de Teste 2", Date = new DateOnly(2025, 12, 25) }
            };
            //  Preparação do ambiente de teste
            var optionsdb = new DbContextOptionsBuilder<FeriadosDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            // Banco de dados em memória para simular o contexto
            var dbContext = new FeriadosDbContext(optionsdb);
            dbContext.Feriados.AddRange(feriadosFalsos);
            await dbContext.SaveChangesAsync();

            // Configuração simulada para NagerDateApiSettings
            var nagerDateApiSettings = new FeriadosNacionaisAPI.Api.Configuration.NagerDateApiSettings
            {
                BaseUrl = NagerDateApiBaseUrl
            };
            var options = Microsoft.Extensions.Options.Options.Create(nagerDateApiSettings);

            // Mock do HttpClient, pois não é necessário para o teste atual
            var mockHttpClient = new Mock<HttpClient>();
            var feriadoService = new FeriadoService(mockHttpClient.Object, dbContext, options);

            // Realiza a chamada ao método que será testado
            var resultado = await feriadoService.ObterFeriadosAsync();

            // Verificação dos resultados
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Feriado de Teste 1", resultado[0].Name);
        }
    }
}