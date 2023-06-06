using back_courrier.Models;
using Microsoft.EntityFrameworkCore;
using back_courrier.Data;

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

        public IQueryable<Historique> ListeCourrierBaseQuery()
        {
            /*            var query = _context.Historique
                                .Include(h => h.CourrierDestinataire)
                                    .ThenInclude(cd => cd.Courrier)
                                        .ThenInclude(c => c.ExpediteurInterne)
                                .Include(h => h.CourrierDestinataire)
                                    .ThenInclude(cd => cd.Courrier)
                                        .ThenInclude(c => c.Recepteur)
                                .Include(h => h.CourrierDestinataire)
                                    .ThenInclude(cd => cd.Departement)
                                .Include(h => h.Statut)
                                .OrderByDescending(h => h.Id);
                        *//*                    .GroupBy(h => h.IdCourrierDestinataire)
                                            .Select(g => g.OrderByDescending(h => h.Id).FirstOrDefault());*//*
                        return query;*/

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
                .OrderByDescending(h => h.Id);

            return query;
        }

        public IList<Historique> ListeCourrierCoursier(Utilisateur employe)
        {
            var query = ListeCourrierBaseQuery()
                .Where(h => h.IdResponsable == employe.Id
                && h.Statut.Code == _configuration["Constants:Statut:TransCour"])
                .ToList();

            return query;
        }

        public IList<Historique> ListeCourrierReceptionniste()
        {
            return ListeCourrierBaseQuery()
               /* .Where(h => h.Statut.Code == _configuration["Constants:Statut:TransSec"])*/
                .ToList();
        }

        public IList<Historique> ListeCourrierSec(Utilisateur employe)
        {
            return ListeCourrierBaseQuery()
                .Where(h => h.CourrierDestinataire.IdDepartementDestinataire == employe.IdDepartement
                && h.Statut.Code == _configuration["Constants:Statut:TransSec"])
                .ToList();
        }

        public IList<Historique> ListeCourrierDir(Utilisateur employe)
        {
            return ListeCourrierBaseQuery()
                .Where(h => h.CourrierDestinataire.IdDepartementDestinataire == employe.IdDepartement
                && h.Statut.Code == _configuration["Constants:Statut:Livre"])
                .ToList();
        }

        public IList<Historique> ListeCourrier(Utilisateur employe)
        {
            if (employe.Poste.Code == _configuration["Constants:Role:RecRole"])
            {
                return ListeCourrierReceptionniste();
            }
            if (employe.Poste.Code == _configuration["Constants:Role:CourRole"])
            {
                return ListeCourrierCoursier(employe);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:SecRole"])
            {
                return ListeCourrierSec(employe);
            }
            if (employe.Poste.Code == _configuration["Constants:Role:DirRole"])
            {
                return ListeCourrierDir(employe);
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
