using back_courrier.Models;

namespace back_courrier.Services
{
    public interface IUtilisateurService
    {
        public Utilisateur GetUtilisateurByPseudo(string pseudo);
    }
}
