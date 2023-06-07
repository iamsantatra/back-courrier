using back_courrier.Models;
using Microsoft.EntityFrameworkCore;
using back_courrier.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace back_courrier.Services
{
    public class CourrierService : ICourrierService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUploadService _fileUploadService;
        private readonly IConfiguration _configuration;

        public CourrierService(ApplicationDbContext context, IUploadService fileUploadService, IConfiguration configuration)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _configuration = configuration;
        }

        public Courrier CreationCourrier(Courrier courrier, Utilisateur employe,
            List<Departement> SelectedDestinataires, IFormFile formFile)
        {
            Statut statusCreer = _context.Statut.Where(s => s.Code == _configuration["Constants:Role:RecRole"]).First();
            courrier.Recepteur = employe;
            courrier.IdReceptionniste = employe.Id;
            courrier.Fichier = _fileUploadService.UploadFileAsync(formFile);
            _context.Courrier.Add(courrier);
            if (!courrier.DateCreation.HasValue)
            {
                courrier.DateCreation = DateTime.Now;
            }
            List<CourrierDestinataire> destinataires = SelectedDestinataires
                .Select(departement => new CourrierDestinataire(courrier, departement, statusCreer, employe)).ToList();
            
            foreach (CourrierDestinataire dest in destinataires)
            {
                dest.Historiques = new Collection<Historique>();
                dest.Historiques.Add(new Historique { Statut = statusCreer });
            }
            courrier.Destinataires = destinataires;
            foreach (CourrierDestinataire dest in destinataires)
            {
                dest.Historiques = new Collection<Historique>();
                dest.Historiques.Add(new Historique { Statut = statusCreer, CourrierDestinataire = dest, Responsable = employe });
            }
            return courrier;
        }

        public IQueryable<CourrierDestinataire> ListeCourrierBaseQuery(int pageNumber, int pageSize, Boolean pagination)
        {

            var query = _context.CourrierDestinataire
                .Include(cd => cd.Courrier)
                    .ThenInclude(c => c.ExpediteurInterne)
                .Include(cd => cd.Courrier)
                    .ThenInclude(c => c.Recepteur)
                .Include(cd => cd.Responsable)
                    .ThenInclude(r => r.Departement)
                .Include(cd => cd.Responsable)
                    .ThenInclude(r => r.Poste)
                .Include(cd => cd.Responsable)
                    .ThenInclude(r => r.Poste)
                .Include(cd => cd.Statut)
                .Include(cd => cd.DepartementDestinataire).AsQueryable();


            if (pagination)
            {
                query = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
                /*query = back_courrier.Helper.Helper.Paginate(query, pageNumber, pageSize);*/
            }

            return query/*.OrderByDescending(h => h.Id)*/;
        }

        public IQueryable<CourrierDestinataire> ListeCourrierReceptionnisteQuery(int pageNumber, int pageSize, Boolean pagination)
        {
            return ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                /* .Where(h => h.Statut.Code == _configuration["Constants:Statut:TransSec"])*/
                /*.ToList()*/;
        }
        public IQueryable<CourrierDestinataire> ListeCourrierCoursierQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            var query = ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                .Where(h => h.IdResponsable == employe.Id
                && h.Statut.Code == _configuration["Constants:Statut:TransCou"])
                /*.ToList()*/;

            return query;
        }

        public IQueryable<CourrierDestinataire> ListeCourrierSecQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            return ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                .Where(h => h.DepartementDestinataire.Id == employe.IdDepartement
                && h.Statut.Code == _configuration["Constants:Statut:TransSec"])
                /*.ToList()*/;
        }

        public IQueryable<CourrierDestinataire> ListeCourrierDirQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            return ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                .Where(h => h.DepartementDestinataire.Id == employe.IdDepartement
                && h.Statut.Code == _configuration["Constants:Statut:Livre"])
                /*.ToList()*/;
        }

        public IQueryable<CourrierDestinataire> ListeCourrierQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            if (employe.Poste.Code == _configuration["Constants:Role:RecRole"])
            {
                return ListeCourrierReceptionnisteQuery(pageNumber, pageSize, pagination);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:CouRole"])
            {
                return ListeCourrierCoursierQuery(employe, pageNumber, pageSize, pagination);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:SecRole"])
            {
                return ListeCourrierSecQuery(employe, pageNumber, pageSize, pagination);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:DirRole"])
            {
                return ListeCourrierDirQuery(employe, pageNumber, pageSize, pagination);
            }
            return null;
        }

        public IList<CourrierDestinataire> ListeCourrier(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            IList <CourrierDestinataire> listeCourrier = ListeCourrierQuery(employe, pageNumber, pageSize, pagination).ToList();
            return listeCourrier;
        }

        public CourrierDestinataire GetDetailsCourrier(int IdCourrierDestinataire)
        {
            CourrierDestinataire detailsCourrier = ListeCourrierBaseQuery(0, 0, false).First(cd => cd.Id == IdCourrierDestinataire);
            /*            CourrierDestinataire detailsCourrier = _context.Historique
                            .Include(h => h.CourrierDestinataire)
                            .Include(h => h.Statut)
                            .Include(h => h.CourrierDestinataire)
                                .ThenInclude(cd => cd.Courrier)
                            .Include(h => h.CourrierDestinataire)
                                .ThenInclude(cd => cd.DepartementDestinataire)
                            .Include(h => h.CourrierDestinataire)
                                    .ThenInclude(cd => cd.Courrier)
                                        .ThenInclude(c => c.Recepteur)
                            .Where(h => h.IdCourrierDestinataire == IdCourrierDestinataire)
                            .First();*/
            return detailsCourrier;
        }
        public CourrierDestinataire TransfertCourrier(CourrierDestinataire courrierDestinataire)
        {
            CourrierDestinataire cd = courrierDestinataire;
            if (cd != null)
            {
                cd.IdStatut++;
                cd.IdResponsable = cd.IdResponsable;
                var defaultDate = DateTime.UtcNow;

                Historique historique = new Historique
                {
                    IdCourrierDestinataire = cd.Id,
                    IdStatut = cd.IdStatut,
                    IdResponsable = cd.IdResponsable,
                    DateHistorique = cd.DateMaj
                };
                if (!cd.DateMaj.HasValue)
                {
                    cd.DateMaj = defaultDate;
                    historique.DateHistorique = DateTime.UtcNow;
                }
                _context.CourrierDestinataire.Update(cd);
                _context.Historique.Add(historique);
                _context.SaveChanges();
            }
            return cd;
        }

        public byte[] ExportPDF(IList<CourrierDestinataire> cds)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate()); // Set the page size to landscape

                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                // Create the table and set its properties
                PdfPTable table = new PdfPTable(10); // Adjust the number of columns as needed
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f }); // Adjust the column widths as needed

                // Add the table headers
                PdfPCell headerCell1 = new PdfPCell(new Phrase("ID", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell2 = new PdfPCell(new Phrase("Objet", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell3 = new PdfPCell(new Phrase("Reference", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                /*PdfPCell headerCell4 = new PdfPCell(new Phrase("Commentaire", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));*/
                PdfPCell headerCell5 = new PdfPCell(new Phrase("Date creation", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell6 = new PdfPCell(new Phrase("Expediteur", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell7 = new PdfPCell(new Phrase("Recepteur", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell8 = new PdfPCell(new Phrase("Flag", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell9 = new PdfPCell(new Phrase("Destinataire", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell10 = new PdfPCell(new Phrase("Status", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell headerCell11 = new PdfPCell(new Phrase("Responsable", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

                table.AddCell(headerCell1);
                table.AddCell(headerCell2);
                table.AddCell(headerCell3);
                /*table.AddCell(headerCell4);*/
                table.AddCell(headerCell5);
                table.AddCell(headerCell6);
                table.AddCell(headerCell7);
                table.AddCell(headerCell8);
                table.AddCell(headerCell9);
                table.AddCell(headerCell10);
                table.AddCell(headerCell11);

                // Add the historique data to the table
                foreach (var cd in cds)
                {
                    PdfPCell cell1 = new PdfPCell(new Phrase(cd.Id.ToString()));
                    PdfPCell cell2 = new PdfPCell(new Phrase(cd.Courrier.Objet));
                    PdfPCell cell3 = new PdfPCell(new Phrase(cd.Courrier.Reference));
                    /*PdfPCell cell4 = new PdfPCell(new Phrase(cd.Courrier.Commentaire));*/
                    PdfPCell cell5 = new PdfPCell(new Phrase(cd.Courrier.DateCreation.ToString()));
                    PdfPCell cell6 = null;
                    if (cd.Courrier.ExpediteurInterne != null)
                    {
                        cell6 = new PdfPCell(new Phrase(cd.Courrier.ExpediteurInterne.Designation));
                    }
                    else
                    {
                        cell6 = new PdfPCell(new Phrase(cd.Courrier.ExpediteurExterne));
                    }
                    PdfPCell cell7 = new PdfPCell(new Phrase(cd.Courrier.Recepteur.Nom));
                    PdfPCell cell8 = new PdfPCell(new Phrase(cd.Courrier.Flag));
                    PdfPCell cell9 = new PdfPCell(new Phrase(cd.DepartementDestinataire.Designation));
                    PdfPCell cell10 = new PdfPCell(new Phrase(cd.Statut.Designation));
                    PdfPCell cell11 = new PdfPCell(new Phrase(cd.Responsable.Nom));
                    table.AddCell(cell1);
                    table.AddCell(cell2);
                    table.AddCell(cell3);
                    /*table.AddCell(cell4);*/
                    table.AddCell(cell5);
                    table.AddCell(cell6);
                    table.AddCell(cell7);
                    table.AddCell(cell8);
                    table.AddCell(cell9);
                    table.AddCell(cell10);
                    table.AddCell(cell11);
                }

                // Add the table to the document
                document.Add(table);

                document.Close();
                return stream.ToArray();
            }
        }

        public IQueryable<CourrierDestinataire> QuerySearchBuilder(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            CourrierDestinataire courrierDestinataire, Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            var query = this.ListeCourrierQuery(employe, pageNumber, pageSize, pagination);

            /*if (DateCreationStart.HasValue)
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.DateCreation >= DateCreationStart.Value);
            }
            if (DateCreationEnd.HasValue)
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.DateCreation <= DateCreationEnd.Value);
            }
            if (DateCreationStart.HasValue && DateCreationEnd.HasValue)
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.DateCreation >= DateCreationStart.Value 
                    && h.CourrierDestinataire.Courrier.DateCreation <= DateCreationEnd.Value);
            }

            if (!string.IsNullOrEmpty(cd.Courrier.Reference))
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.Reference
                    .Contains(cd.Courrier.Reference));
            }

            if (!string.IsNullOrEmpty(cd.Courrier.Objet))
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.Objet
                    .Contains(cd.Courrier.Objet));
            }

            if (!string.IsNullOrEmpty(cd.Courrier.ExpediteurExterne))
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.ExpediteurExterne
                    .Contains(cd.Courrier.ExpediteurExterne));
            }

            if (!string.IsNullOrEmpty(cd.Courrier.ExpediteurInterne.Designation))
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.ExpediteurInterne.Designation
                    .Contains(cd.Courrier.ExpediteurInterne.Designation));
            }

            if (!string.IsNullOrEmpty(cd.Courrier.Flag))
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.Flag
                    .Contains(cd.Courrier.Flag));
            }

            if (!string.IsNullOrEmpty(cd.Courrier.Commentaire))
            {
                query = query.Where(h => h.CourrierDestinataire.Courrier.Commentaire
                    .Contains(cd.Courrier.Commentaire));
            }
*/
/*            if (!string.IsNullOrEmpty(historique.Statut.Designation))
            {
                query = query.Where(h => h.Statut.Designation.Contains(historique.Statut.Designation));
            }*/
/*
            if (!string.IsNullOrEmpty(cd.DepartementDestinat.Designation))
            {
                query = query.Where(h => h.CourrierDestinataire.Departement.Designation
                    .Contains(cd.Departement.Designation));
            }

            if (!string.IsNullOrEmpty(historique.Utilisateur.Nom))
            {
                query = query.Where(h => h.Utilisateur.Nom.Contains(historique.Utilisateur.Nom));
            }*/

            return query;
        }

        public IList<CourrierDestinataire> ListeRecherche(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            CourrierDestinataire courrierDestinataire, Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            IList<CourrierDestinataire> listeRecherche = this.QuerySearchBuilder(DateCreationStart, DateCreationEnd,
            courrierDestinataire, employe, pageNumber, pageSize, pagination).ToList();
            return listeRecherche;
        }
    }
}
