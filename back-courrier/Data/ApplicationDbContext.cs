using back_courrier.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<Statut> Statut { get; set; }
        public DbSet<Courrier> Courrier { get; set; }
        public DbSet<CourrierDestinataire> CourrierDestinataire { get; set; }
        public DbSet<Historique> Historique { get; set; }
        /*public DbSet<VueListeCourrier> VueListeCourrier { get; set; }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Add other configuration options as needed
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add initial data
            Departement dp1 = new() { Id = 1, Designation = "Ressource humaine" };
            Departement dp2 = new() { Id = 2, Designation = "Finance" };
            Departement dp3 = new() { Id = 3, Designation = "SI" };
            
            modelBuilder.Entity<Departement>().HasData(dp1, dp2, dp3);

            Poste p1 = new() { Id = 1, Designation = "receptionniste" };
            Poste p2 = new() { Id = 2, Designation = "coursier" };
            Poste p3 = new() { Id = 3, Designation = "secretaire" };
            Poste p4 = new() { Id = 4, Designation = "directeur" };

            modelBuilder.Entity<Poste>().HasData(p1, p2, p3, p4);

            Utilisateur u1 = new() { Id = 1, Nom = "receptionniste", MotDePasse = "1234", PosteId = 1, DepartementId = 1 };
            Utilisateur u2 = new() { Id = 2, Nom = "coursier", MotDePasse = "1234", PosteId = 2, DepartementId = 1 };
            Utilisateur u3 = new() { Id = 3, Nom = "secretaire", MotDePasse = "1234", PosteId = 3, DepartementId = 1 };
            Utilisateur u4 = new() { Id = 4, Nom = "directeur", MotDePasse = "1234", PosteId = 4, DepartementId = 1 };

            modelBuilder.Entity<Utilisateur>().HasData(u1, u2, u3, u4);

            modelBuilder.Entity<Statut>().HasData(
                new Statut { Id = 1, Designation = "reçu par le receptionniste" },
                new Statut { Id = 2, Designation = "transferé au coursier" },
                new Statut { Id = 3, Designation = "transferé au sécrétaire" },
                new Statut { Id = 4, Designation = "livré" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
