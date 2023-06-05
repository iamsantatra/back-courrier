using back_courrier.Models;
using System.Security.Claims;

namespace back_courrier.Services
{
    public interface IUtilisateurService
    {
        public Utilisateur GetUtilisateurByPseudo(string pseudo);
        public Utilisateur GetUtilisateurByClaim(ClaimsPrincipal currentUser);   
    }
}
