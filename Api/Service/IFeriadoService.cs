namespace FeriadosNacionaisAPI.Api.Services
{
    public interface IFeriadoService
    {
        // Método para obter a lista de feriados
        Task<List<Feriado>> ObterFeriadosAsync();
    }
}