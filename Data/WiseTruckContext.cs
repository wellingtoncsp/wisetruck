using Microsoft.EntityFrameworkCore;
using WiseTruck.API.Models;

namespace WiseTruck.API.Data
{
    public class WiseTruckContext : DbContext
    {
        public WiseTruckContext(DbContextOptions<WiseTruckContext> options)
            : base(options)
        {
        }

        public DbSet<Caminhao> Caminhoes { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<Viagem> Viagens { get; set; }
        public DbSet<Pedagio> Pedagios { get; set; }
        public DbSet<Abastecimento> Abastecimentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Caminhao>().ToTable("tb_caminhoes");
            modelBuilder.Entity<Motorista>().ToTable("tb_motoristas");
            modelBuilder.Entity<Viagem>().ToTable("tb_viagens");
            modelBuilder.Entity<Pedagio>().ToTable("tb_pedagios");
            modelBuilder.Entity<Abastecimento>().ToTable("tb_abastecimentos");
        }
    }
}