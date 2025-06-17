using Microsoft.EntityFrameworkCore;

namespace FeriadosNacionaisAPI.Data
{
    // Classe FeriadoDbContext que herda de DbContext
    public class FeriadoDbContext : DbContext
    {
        // Construtor que recebe opções de configuração do DbContext
        public FeriadoDbContext(DbContextOptions<FeriadoDbContext> options) : base(options)
        {
        }

        // Propriedade DbSet para acessar a tabela de feriados
        public DbSet<Feriado> Feriados { get; set; }
    }
}