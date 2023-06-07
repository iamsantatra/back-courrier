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
        public DbSet<Statut> Statut { get; set; }
        public DbSet<Courrier> Courrier { get; set; }
        public DbSet<CourrierDestinataire> CourrierDestinataire { get; set; }
        public DbSet<Historique> Historique { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Add other configuration options as needed
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utilisateur>()
                .HasOne(e => e.Poste);

            modelBuilder.Entity<Utilisateur>()
                .HasOne(e => e.Departement);
            //Courrier
            modelBuilder.Entity<Courrier>()
                .HasOne(e => e.Recepteur);

            modelBuilder.Entity<Courrier>()
               .HasOne(e => e.ExpediteurInterne);

/*            modelBuilder.Entity<CourrierDestinataire>()
                .HasKey(cd => new { cd.IdCourrier, cd.IdDepartementDestinataire });*/

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Courrier)
                .WithMany(c => c.Destinataires)
                .HasForeignKey(cd => cd.IdCourrier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Departement)
                .WithMany(d => d.CourrierDestinataires)
                .HasForeignKey(cd => cd.IdDepartementDestinataire)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Historique>()
                .HasOne(cd => cd.CourrierDestinataire)
                .WithMany()
                .HasForeignKey(cd => cd.IdCourrierDestinataire)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Historique>()
                .HasOne(cd => cd.Statut)
                .WithMany()
                .HasForeignKey(cd => cd.IdStatut)
                .OnDelete(DeleteBehavior.NoAction);

            /*            modelBuilder.Entity<CourrierDestinataire>()
                            .HasOne(cd => cd.Coursier)
                            .WithMany()
                            .HasForeignKey(cd => cd.IdCoursier)
                            .OnDelete(DeleteBehavior.NoAction);

                        modelBuilder.Entity<CourrierDestinataire>()
                            .HasOne(cd => cd.Status)
                            .WithMany()
                            .HasForeignKey(cd => cd.IdStatus)
                            .OnDelete(DeleteBehavior.NoAction);*/
            base.OnModelCreating(modelBuilder);
        }
    }
}
