using back_courrier.Data;
using back_courrier.Models;
using System.Net.NetworkInformation;
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

        public List<Utilisateur> GetUtilisateurByRole(string posteCode)
        {
            return _context.Utilisateur.Where(e => e.Poste.Code == posteCode).ToList();
        }

        public List<Utilisateur> GetUtilisateurByRoleAndDepartement(string posteCode, string departement)
        {
            return _context.Utilisateur.Where(e => e.Poste.Code == posteCode && e.Departement.Designation == departement).ToList();
        }

        public List<Utilisateur> GetUtilisateurSuivant(Utilisateur UtilisateurCourant, int IdDepartement, int IdStatut)
        {
            List<Utilisateur>? listProchain = null;
            int PosteCourante = UtilisateurCourant.IdPoste;
            int PosteSuivante = UtilisateurCourant.IdPoste + 1;
            // receptionniste et reçu
            if (PosteCourante == 1 && IdStatut == 1)
            {
                listProchain = _context.Utilisateur.Where(u => u.IdPoste == PosteSuivante).ToList();
            }
            // coursier et transferé au coursier
            else if (PosteCourante == 2 && IdStatut == 2)
            {
                listProchain = _context.Utilisateur.Where(u => u.IdPoste == PosteSuivante && u.IdDepartement == IdDepartement).ToList();
            }
            // sécrétaire et transferé au sécrétaire
            else if (PosteCourante == 3 && IdStatut == 3 && UtilisateurCourant.IdDepartement == IdDepartement)
            {
                listProchain = _context.Utilisateur.Where(u => u.IdPoste == PosteSuivante && u.IdDepartement == IdDepartement).ToList();
            }
            // directeur et transferé au directeur
            else if (PosteCourante == 4 && IdStatut == 4 && UtilisateurCourant.IdDepartement == IdDepartement)
            {
                listProchain = _context.Utilisateur.Where(u => u.IdPoste == PosteSuivante && u.IdDepartement == IdDepartement).ToList();
            }
            return listProchain;
        }
    }
}
