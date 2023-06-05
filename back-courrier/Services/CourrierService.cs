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

        public Courrier creationCourrier(Courrier courrier, Utilisateur employe, 
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

        public IQueryable<CourrierDestinataire> ListeCourrierBaseQuery()
        {
            return _context.CourrierDestinataire
                    .Include(cd => cd.Courrier)
                        .ThenInclude(c => c.ExpediteurInterne)
                    .Include(cd => cd.Courrier)
                        .ThenInclude(c => c.Recepteur)
                    .Include(cd => cd.Courrier)
                        .ThenInclude(c => c.Flag)
                    .Include(cd => cd.Departement)
                    .Include(cd => cd.Historique)
                        .ThenInclude(h => h.Statut)
                    .Include(cd => cd.Historique)
                        .ThenInclude(h => h.Utilisateur);
        }

        public IList<CourrierDestinataire> listeCourrierCoursier(Utilisateur employe)
        {
            throw new NotImplementedException();
        }

        public IList<CourrierDestinataire> listeCourrierReceptionniste()
        {
            return ListeCourrierBaseQuery().ToList();
        }

        public IList<CourrierDestinataire> listeCourrierSecDir(Utilisateur employe)
        {
            throw new NotImplementedException();
        }
    }
}
