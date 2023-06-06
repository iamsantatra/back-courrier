using back_courrier.Data;
using back_courrier.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Geom;
using System.Drawing.Printing;
using back_courrier.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

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
        public IList<Historique> listeCourrier { get; set; }
        public IList<Utilisateur> listeCoursier { get; set; }
        public int _pageNumber { get; set; } = 1;
        public int _totalPages { get; set; }
        public int _pageSize { get; set; } = 10;
        public async Task OnGetAsync()
        {
            string dirRole = _configuration.GetValue<string>("Constants:Role:DirRole");
            string secRole = _configuration.GetValue<string>("Constants:Role:SecRole");
            string courRole = _configuration.GetValue<string>("Constants:Role:CourRole");
            string recRole = _configuration.GetValue<string>("Constants:Role:RecRole");
            ViewData["DirRole"] = dirRole;
            ViewData["SecRole"] = secRole;
            ViewData["CourRole"] = courRole;
            ViewData["RecRole"] = recRole;
           /* listeCoursier = _employeService.GetUtilisateurByRole(_configuration.GetValue<string>("Constants:Role:CourRole"));
            ViewData["Coursiers"] = new SelectList(listeCoursier, "Id", "Nom");*/

            if (_context.Courrier != null)
            {
                _currentUser = _employeService.GetUtilisateurByClaim(User);
                _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);
                listeCourrier = _courrierService.ListeCourrier(_currentUser);
            }
        }
/*
        [BindProperty]
        public VueListeCourrier VueListeCourrier { get; set; }*/

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

        /*public void GetListCourrierModel(int pageNumber, int pageSize)
        {
            int skipAmount = (pageNumber - 1) * pageSize;

            // Retrieve a subset of VueListeCourrier records based on the page number and page size
            IQueryable<VueListeCourrier> query = GetListCourrierQuery();

            // Paginer les résultats
            var paginatedQuery = query.Skip(skipAmount).Take(pageSize);

            // Exécuter la requête et renvoyer les résultats
            ListeCourrier = paginatedQuery.ToList();
        }*/

        /*public IQueryable<VueListeCourrier> GetListCourrierQuery()
        {
            // Retrieve a subset of VueListeCourrier records based on the page number and page size
            var query = _context.VueListeCourrier
                .FromSqlRaw("SELECT * FROM VueListeCourrier")
                .AsQueryable();

            // Exécuter la requête et renvoyer les résultats
            return query;
        }*/

        /*public int CalculateTotalPages(int pageSize)
        {
            int totalRecords = _context.VueListeCourrier.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            return totalPages;
        }*/

        /*public FileResult OnPostExport(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(GridHtml)))
            {
                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A4);
                HtmlConverter.ConvertToPdf(stream, pdfDocument);
                pdfDocument.Close();
                return File(byteArrayOutputStream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }*/
        /*public IActionResult OnPostSearch()
        {
            Boolean result = Search(_pageNumber, _pageSize);
            if (!result)
            {
                OnGet(_pageNumber, _pageSize);
            }
            _totalPages = CalculateTotalPages(_pageSize);
            return Page();
        }*/

        /*public Boolean Search(int pageNumber, int pageSize)
        {
            int skipAmount = (pageNumber - 1) * pageSize;
            var query = GetListCourrierQuery();
            Boolean isSearch = false;

            if (DateCreationStart.HasValue)
            {
                query = query.Where(c => c.DateCreation >= DateCreationStart.Value);
                isSearch = true;
            }
            if (DateCreationEnd.HasValue)
            {
                query = query.Where(c => c.DateCreation <= DateCreationEnd.Value);
                isSearch = true;
            }
            if (DateCreationStart.HasValue && DateCreationEnd.HasValue)
            {
                query = query.Where(c => c.DateCreation >= DateCreationStart.Value && c.DateCreation <= DateCreationEnd.Value);
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.Reference))
            {
                query = query.Where(c => c.Reference.Contains(VueListeCourrier.Reference));
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.Objet))
            {
                query = query.Where(c => c.Objet.Contains(VueListeCourrier.Objet));
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.Expediteur))
            {
                query = query.Where(c => c.Expediteur.Contains(VueListeCourrier.Expediteur));
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.Flag))
            {
                query = query.Where(c => c.Flag.Contains(VueListeCourrier.Flag));
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.Commentaire))
            {
                query = query.Where(c => c.Commentaire.Contains(VueListeCourrier.Commentaire));
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.NomStatut))
            {
                query = query.Where(c => c.NomStatut.Contains(VueListeCourrier.NomStatut));
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.NomDepartement))
            {
                query = query.Where(c => c.NomDepartement.Contains(VueListeCourrier.NomDepartement));
                isSearch = true;
            }

            if (!string.IsNullOrEmpty(VueListeCourrier.NomResponsable))
            {
                query = query.Where(c => c.NomResponsable.Contains(VueListeCourrier.NomResponsable));
                isSearch = true;
            }

            var paginatedQuery = query.Skip(skipAmount).Take(pageSize);
            this.ListeCourrier = paginatedQuery.ToList();
            return isSearch;
        }*/
    }
}
