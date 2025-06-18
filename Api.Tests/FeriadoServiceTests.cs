using FeriadosNacionaisAPI.Api.Services; // Para acessar IFeriadoService
using Moq; // Para usar Mocks
using FeriadosNacionaisAPI.Data; // Para acessar DbContext
using Microsoft.EntityFrameworkCore;
using Xunit; // Para usar EF Core em memória

namespace Api.Tests
{
    public class FeriadoServiceTests
    {
        [Fact]
        public async Task ObterFeriadosAsync_QuandoCacheExiste_RetornaFeriadosDoBancoDeDados()
        {
            // Arrange (Organizar)
            // -------------------

            // 1. Criar uma lista falsa de feriados que simula os dados no banco
            var feriadosFalsos = new List<Feriado>
            {
                new Feriado { Id = 1, Name = "Feriado de Teste 1", Date = new DateOnly(2025, 1, 1) },
                new Feriado { Id = 2, Name = "Feriado de Teste 2", Date = new DateOnly(2025, 12, 25) }
            };

            // 2. Configurar um DbContext em memória.
            // Isso cria um "banco de dados falso" que só existe durante este teste.
            var options = new DbContextOptionsBuilder<FeriadosDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // 3. Adicionar nossos dados falsos ao banco de dados em memória
            using (var context = new FeriadosDbContext(options))
            {
                context.Feriados.AddRange(feriadosFalsos);
                context.SaveChanges();
            }

            // 4. Criar um mock do HttpClient (não vamos usá-lo neste teste, mas o serviço precisa dele)
            var mockHttpClient = new Mock<HttpClient>();

            // 5. Criar a instância do nosso serviço, passando o DbContext em memória e o HttpClient mockado
            // Usaremos um novo contexto para o teste para garantir que não haja "trapaças"
            using var testContext = new FeriadosDbContext(options);
            var feriadoService = new FeriadoService(testContext, mockHttpClient.Object);
            
            // Act (Agir)
            // ----------
            // Executar o método que queremos testar
            var resultado = await feriadoService.ObterFeriadosAsync();

            // Assert (Verificar)
            // ------------------
            // Verificar se o resultado é o esperado
            Assert.NotNull(resultado); // O resultado não deve ser nulo
            Assert.Equal(2, resultado.Count); // Devemos receber exatamente 2 feriados
            Assert.Equal("Feriado de Teste 1", resultado.First().Name); // O nome do primeiro deve ser o esperado
        }
}
}