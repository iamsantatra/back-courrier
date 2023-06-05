using back_courrier.Data;
using back_courrier.Models;
using System.Security.Claims;

namespace back_courrier.Services
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly ApplicationDbContext _context;
        public UtilisateurService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Utilisateur GetUtilisateurByClaim(ClaimsPrincipal currentUser)
        {
            string pseudo = currentUser.Identity.Name;
            return GetUtilisateurByPseudo(pseudo);
        }

        public Utilisateur GetUtilisateurByPseudo(string pseudo)
        {
            return _context.Utilisateur.FirstOrDefault(u => u.Pseudo == pseudo);
        }
    }
}
