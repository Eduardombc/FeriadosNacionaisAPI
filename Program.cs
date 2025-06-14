using System.Text.Json;

//usado apenas class e não public class para manter o escopo local
class Program
{
    public static async Task Main(string[] args)
    {
        // Cria uma instância de HttpClient
        var httpClient = new HttpClient();
        // Pega feriados nacionais do Brasil de 2025 e salva dentro da variável json
        string json = await httpClient.GetStringAsync("https://date.nager.at/api/v3/PublicHolidays/2025/BR");
        // Para deserializar o JSON, usamos a biblioteca System.Text.Json
        var feriados = JsonSerializer.Deserialize<List<Feriado>>(json);

        // Exibe os feriados
        foreach (var feriado in feriados)
        {
            Console.WriteLine($"Data: {feriado.Date}, Nome Local: {feriado.LocalName}, Nome: {feriado.Name}, Código do País: {feriado.CountryCode}, Fixo: {feriado.Fixed}, Global: {feriado.Global}, Condados: {feriado.Counties}, Ano de Lançamento: {feriado.LaunchYear}, Tipo: {feriado.Type}");
        }
        // Aguarda a entrada do usuário antes de fechar o console
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}