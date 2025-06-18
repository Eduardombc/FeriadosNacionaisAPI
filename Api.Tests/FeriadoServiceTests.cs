using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using FeriadosNacionaisAPI.Api.models;
using FeriadosNacionaisAPI.Api.Services;
using FeriadosNacionaisAPI.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Api.Tests
{
    public class FeriadoServiceTests
    {
        [Fact]
        public async Task ObterFeriadosAsync_QuandoCacheExiste_RetornaFeriadosDoBancoDeDados()
        {
            var feriadosFalsos = new List<Feriado>
            {
                new Feriado { Id = 1, Name = "Feriado de Teste 1", Date = new DateOnly(2025, 1, 1) },
                new Feriado { Id = 2, Name = "Feriado de Teste 2", Date = new DateOnly(2025, 12, 25) }
            };

            var options = new DbContextOptionsBuilder<FeriadosDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            using (var context = new FeriadosDbContext(options))
            {
                context.Feriados.AddRange(feriadosFalsos);
                context.SaveChanges();
            }

            // Mock do HttpMessageHandler para criar um HttpClient fake
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(handlerMock.Object);

            using var testContext = new FeriadosDbContext(options);
            var feriadoService = new FeriadoService(httpClient, testContext);

            var resultado = await feriadoService.ObterFeriadosAsync();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Feriado de Teste 1", resultado.First().Name);
        }
    }
}