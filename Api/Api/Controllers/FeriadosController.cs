using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using FeriadosNacionaisAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace FeriadosNacionaisAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    // Classe FeriadosController que herda de ControllerBase
    public class FeriadosController : ControllerBase
    {
        // Campo privado para armazenar a instância de HttpClient
        private readonly HttpClient _httpClient;

        // Campo privado para armazenar o contexto do banco de dados
        private readonly FeriadoDbContext _context;

        // Construtor que recebe um HttpClient através da injeção de dependência
        public FeriadosController(HttpClient httpClient, FeriadoDbContext context)
        {
            // Inicializa o HttpClient através da injeção de dependência
            _httpClient = httpClient;
            _context = context;
        }
        [HttpGet]
        // Define o método HTTP GET para obter a lista de feriados
        // Detalhe para o Task<ActionResult<List<Feriado>>>:
        // - Task: Indica que o método é assíncrono e retornará uma tarefa.
        // - ActionResult: Representa o resultado de uma ação em um controlador ASP.NET Core, permitindo retornar diferentes tipos de respostas HTTP.
        public async Task<ActionResult<List<Feriado>>> GetFeriadosAsync()
        {
            if (await _context.Feriados.AnyAsync())
            {
                // Se já existem feriados no banco de dados, retorna a lista de feriados
                var feriadosdoDB = await _context.Feriados.ToListAsync();

                // Retorna um resultado HTTP 200 OK com a lista de feriados do banco de dados
                return Ok(feriadosdoDB);
            }
            // Pega feriados nacionais do Brasil de 2025 e salva dentro da variável json
            string json = await _httpClient.GetStringAsync("https://date.nager.at/api/v3/PublicHolidays/2025/BR");

            // Linha adicionada para depuração
            //Console.WriteLine("JSON recebido: ");
            //Console.WriteLine(json); // Imprime o JSON no console para depuração
            //Console.WriteLine("Fim do JSON recebido.");

            // Define as opções de serialização JSON
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Permite que as propriedades sejam lidas sem diferenciar maiúsculas de minúsculas
            };
            // Para deserializar o JSON, usamos a biblioteca System.Text.Json
            var feriadosdaAPI = JsonSerializer.Deserialize<List<Feriado>>(json, options);
            // - Ok: Retorna um resultado HTTP 200 OK com a lista de feriados.
            
            if (feriadosdaAPI != null && feriadosdaAPI.Any())
            {
                // Se a lista de feriados não for nula e contiver elementos, adiciona os feriados ao banco de dados
                await _context.Feriados.AddRangeAsync(feriadosdaAPI);
                // Salva as alterações no contexto do banco de dados
                await _context.SaveChangesAsync();
            }


            // Adiciona os feriados ao banco de dados
            return Ok(feriadosdaAPI);
        }
    }
}
