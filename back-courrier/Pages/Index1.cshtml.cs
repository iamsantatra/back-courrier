using back_courrier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace back_courrier.Pages
{
    public class Index1 : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string Nom { get; set; }

        [BindProperty]
        public string MotDePasse { get; set; }

        public Index1(ILogger<Index> logger, Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public Utilisateur Utilisateur { get; set; }

        public IActionResult OnPost()
        {
            var utilisateur = _context.Utilisateur.Where(u => u.Nom == Utilisateur.Nom && u.MotDePasse == Utilisateur.MotDePasse).FirstOrDefault();
            if (utilisateur != null)
            {
                // Return to CreationCourrier page
                return RedirectToPage("./Index");
            }
            // Authentication failed, show error message
            ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect");
            return Page();
        }
    }
}
