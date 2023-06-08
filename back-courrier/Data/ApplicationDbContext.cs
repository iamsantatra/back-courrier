using back_courrier.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace back_courrier.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Flag> Flag { get; set; }
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
                .HasOne(c => c.Flag)
                .WithMany()
                .HasForeignKey(c => c.IdFlag)
                .OnDelete(DeleteBehavior.NoAction);

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
                .HasOne(cd => cd.DepartementDestinataire)
                .WithMany(d => d.CourrierDestinataires)
                .HasForeignKey(cd => cd.IdDepartementDestinataire)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Statut)
                .WithMany()
                .HasForeignKey(cd => cd.IdStatut)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Responsable)
                .WithMany()
                .HasForeignKey(cd => cd.IdResponsable)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Historique>()
                .HasOne(h => h.CourrierDestinataire)
                .WithMany()
                .HasForeignKey(h => h.IdCourrierDestinataire)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Historique>()
                .HasOne(h => h.Statut)
                .WithMany()
                .HasForeignKey(h => h.IdStatut)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Historique>()
                .HasOne(h => h.Responsable)
                .WithMany()
                .HasForeignKey(h => h.IdResponsable)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public void SeedData()
        {
            var flagsS = new List<Flag>
            {
                new Flag{Designation="normal" },
                new Flag{Designation="important" },
                new Flag{Designation="urgent" }
            };
            flagsS.ForEach(p => Flag.Add(p));

            var postes = new List<Poste>
            {
                new Poste{Code="REC", Designation="Receptionniste" },
                new Poste{Code="COU", Designation="Coursier" },
                new Poste{Code="SEC", Designation="Secretaire" },
                new Poste{Code="DIR", Designation="Directeur" }
            };
            postes.ForEach(p => Poste.Add(p));

            var departements = new List<Departement>
            {
                new Departement{Designation="RH"},
                new Departement{Designation="Finance"},
                new Departement{Designation="SI"}
            };
            departements.ForEach(r => Departement.Add(r));

            var users = new List<Utilisateur>
            {
                new Utilisateur {Nom="receptionniste", Pseudo="REC", IdPoste=1, IdDepartement=1, MotDePasse="1234"},
                new Utilisateur {Nom="coursier", Pseudo="COU", IdPoste=2, IdDepartement=1, MotDePasse="1234"},
                new Utilisateur {Nom="secretaire RH", Pseudo="SECRH", IdPoste=3, IdDepartement=1, MotDePasse="1234"},
                new Utilisateur {Nom="directeur RH", Pseudo="DIRRH", IdPoste=4, IdDepartement=1, MotDePasse="1234"},
                new Utilisateur {Nom="secretaire F", Pseudo="SECF", IdPoste=3, IdDepartement=2, MotDePasse="1234"},
                new Utilisateur {Nom="directeur F", Pseudo="DIRF", IdPoste=4, IdDepartement=2, MotDePasse="1234"},
                new Utilisateur {Nom="secretaire SI", Pseudo="SECSI", IdPoste=3, IdDepartement=3, MotDePasse="1234"},
                new Utilisateur {Nom="directeur SI", Pseudo="DIRSI", IdPoste=4, IdDepartement=3, MotDePasse="1234"}
            };
            users.ForEach(r => Utilisateur.Add(r));

            var statuts = new List<Statut>
            {
                new Statut{Code="REC", Designation="reçu par le receptionniste"},
                new Statut{Code="COU", Designation="transferé au coursier"},
                new Statut{Code="SEC", Designation="transferé au sécrétaire"},
                new Statut{Code="DIR", Designation="livré"}
            };
            statuts.ForEach(r => Statut.Add(r));
            SaveChanges();

            var random = new Random();

            for (int i = 0; i < 50; i++)
            {
                var courrier = new Courrier
                {
                    Reference = "REF" + i.ToString("000"),
                    Objet = "Objet du courrier " + i.ToString(),
                    DateCreation = DateTime.Now.AddDays(-random.Next(1, 30)),
                    ExpediteurExterne = "Expediteur externe " + i.ToString(),
                    IdFlag = random.Next(1, 3),
                    Commentaire = "Commentaire du courrier " + i.ToString(),
                    Fichier = "Chemin du fichier " + i.ToString(),
                    IdReceptionniste = 1,
                    /*IdExpediteur = expediteursInternes[random.Next(0, expediteursInternes.Count)].IdDepartement,*/
                };
                
                Courrier.Add(courrier);
                SaveChanges();

                var destinataires = new List<CourrierDestinataire>
                {
                    new CourrierDestinataire
                    {
                        IdCourrier = courrier.Id,
                        IdDepartementDestinataire = 2,
                        IdStatut = 1,
                        IdResponsable = 1,
                        // Ajoutez d'autres propriétés selon vos besoins
                    },
                    new CourrierDestinataire
                    {
                        IdCourrier = courrier.Id,
                        IdDepartementDestinataire = 3,
                        IdStatut = 1,
                        IdResponsable = 1,
                        // Ajoutez d'autres propriétés selon vos besoins
                    },
                };

                CourrierDestinataire.AddRange(destinataires);
                SaveChanges();

                var historiques = destinataires.Select(destinataire => new Historique
                {
                    IdCourrierDestinataire = destinataire.Id,
                    IdStatut = 1,
                    IdResponsable = 1,
                    // Ajoutez d'autres propriétés selon vos besoins
                }).ToList();

                Historique.AddRange(historiques);
            }

            SaveChanges();

        }
    }
}
