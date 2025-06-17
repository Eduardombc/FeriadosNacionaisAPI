using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FeriadosNacionaisAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    // Classe FeriadosController que herda de ControllerBase
    public class FeriadosController : ControllerBase
    {
        // Campo privado para armazenar a instância de HttpClient
        private readonly HttpClient _httpClient;

        // Construtor que recebe um HttpClient através da injeção de dependência
        public FeriadosController(HttpClient httpClient)
        {
            // Inicializa o HttpClient através da injeção de dependência
            _httpClient = httpClient;
        }
        [HttpGet]
        // Define o método HTTP GET para obter a lista de feriados
        // Detalhe para o Task<ActionResult<List<Feriado>>>:
        // - Task: Indica que o método é assíncrono e retornará uma tarefa.
        // - ActionResult: Representa o resultado de uma ação em um controlador ASP.NET Core, permitindo retornar diferentes tipos de respostas HTTP.
        public async Task<ActionResult<List<Feriado>>> GetFeriadosAsync()
        {
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
            var feriados = JsonSerializer.Deserialize<List<Feriado>>(json, options);
            // - Ok: Retorna um resultado HTTP 200 OK com a lista de feriados.
            return Ok(feriados);

        }
    }
}
