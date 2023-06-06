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
                    .Include(h => h.Statut);
        }

        public IList<Historique> ListeCourrierCoursier(Utilisateur employe)
        {
            throw new NotImplementedException();
        }

        public IList<Historique> ListeCourrierReceptionniste()
        {
            return ListeCourrierBaseQuery().ToList();
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
            }/*
            if (employe.Poste.code == _configuration["Constants:Role:CouRole"])
            {
                return listeCourrierCoursier(employe);
            }
            if ((employe.Poste.code == _configuration["Constants:Role:SecRole"]) || (employe.Poste.code == _configuration["Constants:Role:DirRole"]))
            {
                return listeCourrierSecDir(employe);
            }*/
            return null;
        }
    }
}
