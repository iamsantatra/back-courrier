using back_courrier.Models;
using Microsoft.EntityFrameworkCore;

namespace back_courrier.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Poste> Poste { get; set; }
        public DbSet<Departement> Departement { get; set; }
        public DbSet<Utilisateur> Utilisateur { get; set; }

        public DbSet<CourrierDestinataire> CourrierDestinataire { get; set; }
        public DbSet<Courrier> Courrier { get; set; }
        public DbSet<Historique> Historique { get; set; }
        public DbSet<VueListeCourrier> VueListeCourrier { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Add other configuration options as needed
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Poste);
        }
    }
}
