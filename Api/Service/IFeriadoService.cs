using FeriadosNacionaisAPI.Api.Models;


namespace FeriadosNacionaisAPI.Api.Services.Interfaces
{
    public interface IFeriadoService
    {
        // Método para obter a lista de feriados
        Task<List<Feriado>> ObterFeriadosAsync();
    }
}