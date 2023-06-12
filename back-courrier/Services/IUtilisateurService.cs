using back_courrier.Models;
using System.Security.Claims;

namespace back_courrier.Services
{
    public interface IUtilisateurService
    {
        public Utilisateur GetUtilisateurByPseudo(string pseudo);
        public Utilisateur GetUtilisateurByid(int id);
        public Utilisateur GetUtilisateurByClaim(ClaimsPrincipal currentUser);
        public List<Utilisateur> GetUtilisateurByRole(string posteCode);
        public List<Utilisateur> GetUtilisateurByRoleAndDepartement(string posteCode, string departement); 
        public List<Utilisateur> GetUtilisateurSuivant(Utilisateur UtilisateurCourant, int IdDepartement, int IdStatut);
        public List<Utilisateur> GetCoursier();
        public List<Utilisateur> GetSecretaireByDep(int idDepartement);
        public List<Utilisateur> GetDirecteurByDep(int idDepartement);
    }
}
