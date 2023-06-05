using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace back_courrier.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string Pseudo { get; set; }
        [BindProperty]
        public string MotDePasse { get; set; }

        public IndexModel(ILogger<Index> logger, Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Models.Utilisateur Utilisateur { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var UtilisateurConn = _context.Utilisateur.Where(u => u.Pseudo == Utilisateur.Pseudo && u.MotDePasse == Utilisateur.MotDePasse).FirstOrDefault();
            if (UtilisateurConn != null)
            {
                UtilisateurConn.Poste = _context.Poste.Find(UtilisateurConn.IdPoste);
                // Save the object utilisateur in session
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, UtilisateurConn.Pseudo),
                    new Claim(ClaimTypes.Role, UtilisateurConn.Poste.Code),
                };

                var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(claimsIdentity));
                // Return to CreationCourrier page
                /*return RedirectToPage("./ListeCourrier");*/
                return RedirectToPage("/CreationCourrier");
            }
            // Authentication failed, show error message
            ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect");
            return Page();
        }
    }
}
