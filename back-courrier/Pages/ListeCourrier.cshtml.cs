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

namespace back_courrier.Pages
{
    public class ListeCourrierModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<VueListeCourrier> ListeCourrier { get; set; }
        public int _pageNumber { get; set; } = 1;
        public int _totalPages { get; set; }

        public int _pageSize { get; set; } = 10;

        public void OnGet(int pageNumber = 1, int pageSize = 10)
        {
            GetListCourrierModel(pageNumber, pageSize);
            _pageNumber = pageNumber;
            _totalPages = CalculateTotalPages(pageSize);
        }

        [BindProperty]
        public VueListeCourrier VueListeCourrier { get; set; }

        [BindProperty]
        public DateTime? DateCreationStart { get; set; } = null;

        [BindProperty]
        public DateTime? DateCreationEnd { get; set; } = null;
        public ListeCourrierModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GetListCourrierModel(int pageNumber, int pageSize)
        {
            int skipAmount = (pageNumber - 1) * pageSize;

            // Retrieve a subset of VueListeCourrier records based on the page number and page size
            IQueryable<VueListeCourrier> query = GetListCourrierQuery();

            // Paginer les r�sultats
            var paginatedQuery = query.Skip(skipAmount).Take(pageSize);

            // Ex�cuter la requ�te et renvoyer les r�sultats
            ListeCourrier = paginatedQuery.ToList();
        }

        public IQueryable<VueListeCourrier> GetListCourrierQuery()
        {
            // Retrieve a subset of VueListeCourrier records based on the page number and page size
            var query = _context.VueListeCourrier
                .FromSqlRaw("SELECT * FROM VueListeCourrier")
                .AsQueryable();

            // Ex�cuter la requ�te et renvoyer les r�sultats
            return query;
        }

        public int CalculateTotalPages(int pageSize)
        {
            int totalRecords = _context.VueListeCourrier.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            return totalPages;
        }

        public FileResult OnPostExport(string GridHtml)
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
        }
        public IActionResult OnPostSearch()
        {
            Boolean result = Search(_pageNumber, _pageSize);
            if (!result)
            {
                OnGet(_pageNumber, _pageSize);
            }
            _totalPages = CalculateTotalPages(_pageSize);
            return Page();
        }

        public Boolean Search(int pageNumber, int pageSize)
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
        }
    }
}
