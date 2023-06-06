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
            return _context.Historique
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
/*                    .GroupBy(h => h.CourrierDestinataire.Id)
                    .Select(g => g.OrderByDescending(h => h.Id).FirstOrDefault());*/
        }

        public IList<Historique> ListeCourrierCoursier()
        {
            return ListeCourrierBaseQuery()
                .Where(h => h.Statut.Code == _configuration["Constants:Statut:TransCour"])
                .ToList();
        }

        public IList<Historique> ListeCourrierReceptionniste()
        {
            return ListeCourrierBaseQuery()
                .Where(h => h.Statut.Code == _configuration["Constants:Statut:Recu"])
                .ToList();
        }

        public IList<Historique> ListeCourrierSecDir(Utilisateur employe)
        {
            throw new NotImplementedException();
        }

        public IList<Historique> ListeCourrier(Utilisateur employe)
        {
            if (employe.Poste.Code == _configuration["Constants:Role:RecRole"])
            {
                return ListeCourrierReceptionniste();
            }
            if (employe.Poste.Code == _configuration["Constants:Role:CourRole"])
            {
                return ListeCourrierCoursier();
            }
            /*if ((employe.Poste.code == _configuration["Constants:Role:SecRole"]) || (employe.Poste.code == _configuration["Constants:Role:DirRole"]))
            {
                return listeCourrierSecDir(employe);
            }*/
            return null;
        }

        public Historique GetHistoriqueByIdCourrierDestinataire(int IdCourrierDestinataire)
        {
            return ListeCourrierBaseQuery().FirstOrDefault(h => h.CourrierDestinataire.Id == IdCourrierDestinataire);
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
