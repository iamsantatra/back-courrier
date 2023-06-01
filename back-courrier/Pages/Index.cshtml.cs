using back_courrier.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace back_courrier.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string Nom { get; set; }
        [BindProperty]
        public string MotDePasse { get; set; }

        public IndexModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Utilisateur Utilisateur { get; set; }

        public IActionResult OnPost()
        {
            var UtilisateurConn = _context.Utilisateur.Where(u => u.Nom == Utilisateur.Nom && u.MotDePasse == Utilisateur.MotDePasse).FirstOrDefault();
            if (UtilisateurConn != null)
            {
                // Save the object utilisateur in session
                HttpContext.Session.SetObject("utilisateur", UtilisateurConn);
                // Return to CreationCourrier page
                return RedirectToPage("./ListeCourrier");
            }
            // Authentication failed, show error message
            ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect");
            return Page();
        }
    }
}
