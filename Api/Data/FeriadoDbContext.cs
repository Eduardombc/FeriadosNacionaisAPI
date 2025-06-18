using Microsoft.EntityFrameworkCore;
using FeriadosNacionaisAPI.Api.Models;

namespace FeriadosNacionaisAPI.Data
{
    // Classe FeriadoDbContext que herda de DbContext
    public class FeriadosDbContext : DbContext
    {
        // Construtor que recebe opções de configuração do DbContext
        public FeriadosDbContext(DbContextOptions<FeriadosDbContext> options) : base(options)
        {
        }

        // Propriedade DbSet para acessar a tabela de feriados
        public DbSet<Feriado> Feriados { get; set; }
    }
}