using back_courrier.Data;
using back_courrier.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Geom;
using System.Drawing.Printing;
using back_courrier.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using back_courrier.Utils;
using Microsoft.AspNetCore.Http;

namespace back_courrier.Pages
{
    [Authorize]
    public class ListeCourrierModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourrierService _courrierService;
        private readonly IUtilisateurService _employeService;
        private readonly IConfiguration _configuration;
        private Utilisateur _currentUser;
        public Pages<CourrierDestinataire> listeCourrier { get; set; }
        public IList<CourrierDestinataire> listeCourrierSansPag { get; set; }
        /*public IList<Utilisateur> listeCoursier { get; set; }*/
        public int _pageSize { get; } = 10;

        [BindProperty(SupportsGet = true)]
        public DateTime? dateCreationStart { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? dateCreationEnd { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? reference { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? objet { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? expediteurExterne { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? expediteurInterne { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? nomResponsable { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? destinataire { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? commentaire { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? fichier { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? recepteur { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? flag { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? statut { get; set; }


        public async Task OnGetAsync(int pageNumber = 1,
            string reference = null, string objet = null, string expediteurExterne = null, string expediteurInterne = null, 
            string nomResponsable = null, string destinataire = null, string commentaire = null, string fichier = null, 
            string recepteur = null, string flag = null, string statut = null
        )
        {
            string dirRole = _configuration.GetValue<string>("Constants:Role:DirRole");
            string secRole = _configuration.GetValue<string>("Constants:Role:SecRole");
            string courRole = _configuration.GetValue<string>("Constants:Role:CouRole");
            string recRole = _configuration.GetValue<string>("Constants:Role:RecRole");
            ViewData["DirRole"] = dirRole;
            ViewData["SecRole"] = secRole;
            ViewData["CouRole"] = courRole;
            ViewData["RecRole"] = recRole;

            _currentUser = _employeService.GetUtilisateurByClaim(User);
            _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);

            if (!dateCreationStart.HasValue && !dateCreationEnd.HasValue && string.IsNullOrEmpty(reference) && string.IsNullOrEmpty(objet) &&
                string.IsNullOrEmpty(expediteurExterne) && string.IsNullOrEmpty(expediteurInterne) && string.IsNullOrEmpty(nomResponsable) &&
                string.IsNullOrEmpty(destinataire) && string.IsNullOrEmpty(commentaire) && string.IsNullOrEmpty(fichier) &&
                string.IsNullOrEmpty(recepteur) && string.IsNullOrEmpty(flag) && string.IsNullOrEmpty(statut))
            {
                listeCourrier = _courrierService.ListeCourrierPage(_currentUser, pageNumber, _pageSize);
            } 
            else
            {
                listeCourrier = _courrierService.ListeRecherche(dateCreationStart, dateCreationEnd,
                    reference, objet, expediteurExterne, expediteurInterne,
                    nomResponsable, destinataire, commentaire, fichier,
                    recepteur, flag, statut, _currentUser, pageNumber, _pageSize);
            }
        }
        public ListeCourrierModel(ApplicationDbContext context, ICourrierService courrierService,
            IUtilisateurService employeService, IConfiguration configuration)
        {
            _context = context;
            _courrierService = courrierService;
            _employeService = employeService;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetExport(int pageNumber)
        {
            // Call the ExportPDF function to generate the PDF content
            /*await OnGetAsync(pageNumber);*/
            _currentUser = _employeService.GetUtilisateurByClaim(User);
            _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);

            byte[] pdfContent = _courrierService.ExportPDF(_courrierService.ListeRecherche(dateCreationStart, dateCreationEnd,
                    reference, objet, expediteurExterne, expediteurInterne,
                    nomResponsable, destinataire, commentaire, fichier,
                    recepteur, flag, statut, _currentUser, pageNumber, _pageSize).Liste);
            // Generate a unique file name
            string fileName = "liste-courrier-" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".pdf";
            // Set the content type of the file
            string contentType = "application/pdf";
            // Return the file using the File constructor
            return new FileContentResult(pdfContent, contentType)
            {
                FileDownloadName = fileName
            };
            return null;
        }

    }
}
