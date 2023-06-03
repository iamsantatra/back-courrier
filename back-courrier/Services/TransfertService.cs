using back_courrier.Data;
using back_courrier.Models;

namespace back_courrier.Services
{
    public class TransfertService
    {
        public static List<Utilisateur> GetProchainUtilisateur(ApplicationDbContext _context, Utilisateur UtilisateurCourant, int IdDepartement, int IdStatut)
        {
            List<Utilisateur>? listProchain = null;
            int PosteCourante = UtilisateurCourant.IdPoste;
            int PosteSuivante = UtilisateurCourant.IdPoste+1;
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
