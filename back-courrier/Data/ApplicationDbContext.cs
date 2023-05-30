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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Add other configuration options as needed
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*base.OnModelCreating(modelBuilder);*/


            modelBuilder.Entity<Poste>().ToTable("Poste");
            modelBuilder.Entity<Departement>().ToTable("Departement");
            modelBuilder.Entity<Utilisateur>().ToTable("Utilisateur");

            // Add initial data
           /* modelBuilder.Entity<Departement>().HasData(
                new Departement { Id = -1, Designation = "Ressource humaine" },
                new Departement { Id = -2, Designation = "Finance" },
                new Departement { Id = -3, Designation = "SI" }
            );

            modelBuilder.Entity<Poste>().HasData(
                new Poste { Id = -1, Designation = "receptionniste" },
                new Poste { Id = -2, Designation = "coursier" },
                new Poste { Id = -3, Designation = "secretaire" },
                new Poste { Id = -4, Designation = "directeur" }
            );

            modelBuilder.Entity<Utilisateur>().HasData(
                new Utilisateur { Id = -1, Nom = "receptionniste", MotDePasse = "1234", Id_Poste = 1, Id_Departement = -1 },
                new Utilisateur { Id = -2, Nom = "coursier", MotDePasse = "1234", Id_Poste = 2, Id_Departement = -1 },
                new Utilisateur { Id = -3, Nom = "secretaire", MotDePasse = "1234", Id_Poste = 3, Id_Departement = -1 },
                new Utilisateur { Id = -4, Nom = "directeur", MotDePasse = "1234", Id_Poste = 4, Id_Departement = -1 }
            );*/

        }


        public DbSet<back_courrier.Models.Courrier>? Courrier { get; set; }
    }
}
