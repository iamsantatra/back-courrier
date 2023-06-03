using back_courrier.Data;
using back_courrier.Helper;
using back_courrier.Models;
using back_courrier.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace back_courrier.Pages
{
    public class ListeCourrierModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<VueListeCourrier> ListeCourrier { get; set; }
        public void OnGet()
        {
            ListeCourrier = _context.VueListeCourrier.FromSqlRaw("SELECT * FROM VueListeCourrier").ToList();
        }

        public ListeCourrierModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ListeCourrier> GetListCourrierModel()
        {
            return null;
        }

    }
}
