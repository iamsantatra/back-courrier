using back_courrier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace back_courrier.Pages
{
    public class Login : PageModel
    {
        private readonly ILogger<Login> _logger;
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string Nom { get; set; }

        [BindProperty]
        public string MotDePasse { get; set; }

        public Login(ILogger<Login> logger, Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Models.Utilisateur Utilisateur { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var utilisateur = _context.Utilisateur.Where(u => u.Nom == Utilisateur.Nom && u.MotDePasse == Utilisateur.MotDePasse).FirstOrDefault();
            if (utilisateur != null)
            {
                return RedirectToPage("./CreationCourrier");
            }
            
            // Authentication failed, show error message
            ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect");
            return Page();
        }
    }
}
