using back_courrier.Models;
using Microsoft.EntityFrameworkCore;
using back_courrier.Data;
using iText.Kernel.Geom;
using System.Drawing.Printing;

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
            Statut statusCreer = _context.Statut.Where(s => s.Code == "REC").First();
            courrier.Recepteur = employe;
            courrier.IdReceptionniste = employe.Id;
            courrier.Fichier = _fileUploadService.UploadFileAsync(formFile);
            List<CourrierDestinataire> destinataires = SelectedDestinataires
                .Select(departement => new CourrierDestinataire(courrier, departement)).ToList();
            _context.Courrier.Add(courrier);
            courrier.Destinataires = destinataires;
            // get statut where code = "REC"
            Statut statut = _context.Statut.FirstOrDefault(s => s.Code == _configuration["Constants:Role:RecRole"]);
            destinataires.ForEach(courrierDestinataire =>
            {
                _context.Historique.Add(new Historique(courrierDestinataire, statut, employe));
            });
            return courrier;
        }

        public IQueryable<Historique> ListeCourrierBaseQuery(int pageNumber, int pageSize, Boolean pagination)
        {
            var subquery = _context.Historique
                .GroupBy(h => h.IdCourrierDestinataire)
                .Select(g => g.OrderByDescending(h => h.Id).FirstOrDefault())
                .Select(h => h.Id);

            var query = _context.Historique
                .Include(h => h.CourrierDestinataire)
                    .ThenInclude(cd => cd.Courrier)
                        .ThenInclude(c => c.ExpediteurInterne)
                .Include(h => h.CourrierDestinataire)
                    .ThenInclude(cd => cd.Courrier)
                        .ThenInclude(c => c.Recepteur)
                .Include(h => h.CourrierDestinataire)
                    .ThenInclude(cd => cd.Departement)
                .Include(h => h.Statut)
                .Include(h => h.Utilisateur)
                .Where(h => subquery.Contains(h.Id))
                /*.OrderByDescending(h => h.Id)*/;
                /*.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);*/

            if(pagination)
            {
                query = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            }

            return query.OrderByDescending(h => h.Id);
        }

        public IList<Historique> ListeCourrierCoursier(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            var query = ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                .Where(h => h.IdResponsable == employe.Id
                && h.Statut.Code == _configuration["Constants:Statut:TransCour"])
                .ToList();

            return query;
        }

        public IList<Historique> ListeCourrierReceptionniste(int pageNumber, int pageSize, Boolean pagination)
        {
            return ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                /* .Where(h => h.Statut.Code == _configuration["Constants:Statut:TransSec"])*/
                .ToList();
        }

        public IList<Historique> ListeCourrierSec(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            return ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                .Where(h => h.CourrierDestinataire.IdDepartementDestinataire == employe.IdDepartement
                && h.Statut.Code == _configuration["Constants:Statut:TransSec"])
                .ToList();
        }

        public IList<Historique> ListeCourrierDir(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            return ListeCourrierBaseQuery(pageNumber, pageSize, pagination)
                .Where(h => h.CourrierDestinataire.IdDepartementDestinataire == employe.IdDepartement
                && h.Statut.Code == _configuration["Constants:Statut:Livre"])
                .ToList();
        }

        public IList<Historique> ListeCourrier(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination)
        {
            if (employe.Poste.Code == _configuration["Constants:Role:RecRole"])
            {
                return ListeCourrierReceptionniste(pageNumber, pageSize, pagination);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:CourRole"])
            {
                return ListeCourrierCoursier(employe, pageNumber, pageSize, pagination);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:SecRole"])
            {
                return ListeCourrierSec(employe, pageNumber, pageSize, pagination);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:DirRole"])
            {
                return ListeCourrierDir(employe, pageNumber, pageSize, pagination);
            }
            return null;
        }

        public Historique GetHistoriqueByIdCourrierDestinataire(int IdCourrierDestinataire)
        {
            /*Historique detailsHistorique = ListeCourrierBaseQuery().First(h => h.IdCourrierDestinataire == IdCourrierDestinataire);*/
            Historique detailsHistorique = _context.Historique
                .Include(h => h.CourrierDestinataire)
                .Include(h => h.Statut)
                .Include(h => h.CourrierDestinataire)
                    .ThenInclude(cd => cd.Courrier)
                .Include(h => h.CourrierDestinataire)
                    .ThenInclude(cd => cd.Departement)
                .Include(h => h.CourrierDestinataire)
                        .ThenInclude(cd => cd.Courrier)
                            .ThenInclude(c => c.Recepteur)
                .Where(h => h.IdCourrierDestinataire == IdCourrierDestinataire)
                .First();
            return detailsHistorique;
        }
        public Historique TransfertCourrier(Historique historique)
        {
            Historique hResultat = new Historique();
            hResultat.IdStatut = historique.IdStatut + 1;
            hResultat.IdCourrierDestinataire = historique.IdCourrierDestinataire;
            hResultat.IdResponsable = historique.IdResponsable;
            _context.Historique.Add(hResultat);
            return hResultat;
        }
        
    }
}
