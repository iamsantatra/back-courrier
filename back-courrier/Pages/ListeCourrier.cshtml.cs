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
        public IList<CourrierDestinataire> listeCourrier { get; set; }
        public IList<CourrierDestinataire> listeCourrierSansPag { get; set; }
        /*public IList<Utilisateur> listeCoursier { get; set; }*/
        public int _pageNumber { get; set; } = 1;
        public int _totalPages { get; set; }
        public int _pageSize { get; set; } = 10;
        public async Task OnGetAsync(int pageNumber = 1)
        {
            string dirRole = _configuration.GetValue<string>("Constants:Role:DirRole");
            string secRole = _configuration.GetValue<string>("Constants:Role:SecRole");
            string courRole = _configuration.GetValue<string>("Constants:Role:CouRole");
            string recRole = _configuration.GetValue<string>("Constants:Role:RecRole");
            ViewData["DirRole"] = dirRole;
            ViewData["SecRole"] = secRole;
            ViewData["CouRole"] = courRole;
            ViewData["RecRole"] = recRole;
           /* listeCoursier = _employeService.GetUtilisateurByRole(_configuration.GetValue<string>("Constants:Role:CourRole"));
            ViewData["Coursiers"] = new SelectList(listeCoursier, "Id", "Nom");*/

/*            if (_context.Courrier != null)
            {*/
                _currentUser = _employeService.GetUtilisateurByClaim(User);
                _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);
                _pageNumber = pageNumber;
                listeCourrier = _courrierService.ListeCourrier(_currentUser, _pageNumber, _pageSize, true);
                listeCourrierSansPag = _courrierService.ListeCourrier(_currentUser, _pageNumber, _pageSize, false);
                _totalPages = Helper.CalculateTotalPage(listeCourrierSansPag.ToList(), _pageSize);
/*            }*/
        }

        [BindProperty]
        public CourrierDestinataire Historique { get; set; }

        [BindProperty]
        public DateTime? DateCreationStart { get; set; } = null;

        [BindProperty]
        public DateTime? DateCreationEnd { get; set; } = null;
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
            // exportation PDF HTML
            /*// Call the ExportPDF function to generate the PDF content
            byte[][] pdfContent = Helper.ExportPdfHtml(GridHtml);

            // Generate a unique file name
            string fileName = "liste-courrier-"+ DateTime.Now.ToString("MMddyyyyhhmmss") + ".pdf";

            // Set the content type of the file
            string contentType = "application/pdf";

            // Return the file using the File constructor
            return new FileContentResult(pdfContent[0], contentType)
            {
                FileDownloadName = fileName
            };*/
            // exportation PDF HTML

            // Call the ExportPDF function to generate the PDF content
            await OnGetAsync(pageNumber);
            byte[] pdfContent = _courrierService.ExportPDF(listeCourrier);
            // Generate a unique file name
            string fileName = "liste-courrier-" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".pdf";
            // Set the content type of the file
            string contentType = "application/pdf";
            // Return the file using the File constructor
            return new FileContentResult(pdfContent, contentType)
            {
                FileDownloadName = fileName
            };
        }

        public async Task<IActionResult> OnPostSearch()
        {
            /*await OnGetAsync(_pageNumber);*/
            _currentUser = _employeService.GetUtilisateurByClaim(User);
            _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);
            string dirRole = _configuration.GetValue<string>("Constants:Role:DirRole");
            string secRole = _configuration.GetValue<string>("Constants:Role:SecRole");
            string courRole = _configuration.GetValue<string>("Constants:Role:CourRole");
            string recRole = _configuration.GetValue<string>("Constants:Role:RecRole");
            ViewData["DirRole"] = dirRole;
            ViewData["SecRole"] = secRole;
            ViewData["CourRole"] = courRole;
            ViewData["RecRole"] = recRole;
            listeCourrier = _courrierService.ListeRecherche(DateCreationStart, DateCreationEnd, Historique, _currentUser, _pageNumber, _pageSize, false);
            listeCourrierSansPag = _courrierService.ListeRecherche(DateCreationStart, DateCreationEnd, Historique, _currentUser, _pageNumber, _pageSize, false);
            _totalPages = /*Helper.CalculateTotalPage(listeCourrierSansPag.ToList(), _pageSize)*/0;
            return Page();
        }
    }
}
