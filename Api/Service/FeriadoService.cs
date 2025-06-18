using Microsoft.EntityFrameworkCore;
using FeriadosNacionaisAPI.Data;
using System.Text.Json;
using FeriadosNacionaisAPI.Api.Services.Interfaces;
using FeriadosNacionaisAPI.Api.Configuration;
using Microsoft.Extensions.Options;
using FeriadosNacionaisAPI.Api.Models;

namespace FeriadosNacionaisAPI.Api.Services
{
    public class FeriadoService : IFeriadoService
    {
        private readonly FeriadosDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly NagerDateApiSettings _nagerDateApiSettings;

        public FeriadoService(HttpClient httpClient, FeriadosDbContext context, IOptions<NagerDateApiSettings> nagerDateApiSettings)
        {
            _httpClient = httpClient;
            _context = context;
            _nagerDateApiSettings = nagerDateApiSettings.Value;
        }

        public async Task<List<Feriado>> ObterFeriadosAsync()
        {
            if (await _context.Feriados.AnyAsync())
            {
                // Se já existem feriados no banco de dados, retorna a lista de feriados
                var feriadosdoDB = await _context.Feriados.ToListAsync();

                // Retorna um resultado HTTP 200 OK com a lista de feriados do banco de dados
                return (feriadosdoDB);
            }

            string url = $"{_nagerDateApiSettings.BaseUrl}PublicHolidays/2025/BR";
            // Pega feriados nacionais do Brasil de 2025 e salva dentro da variável json
            string json = await _httpClient.GetStringAsync(url);

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
            return feriadosdaAPI ?? new List<Feriado>();
        }
    }
}
