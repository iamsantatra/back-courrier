using back_courrier.Models;

namespace back_courrier.Services
{
    public interface ICourrierService
    {
        Courrier creationCourrier(Courrier courrier, Utilisateur employe, List<Departement> destinataires, IFormFile formFile);
        IQueryable<CourrierDestinataire> ListeCourrierBaseQuery();
        IList<CourrierDestinataire> listeCourrierReceptionniste();
        IList<CourrierDestinataire> listeCourrierCoursier(Utilisateur employe);
        IList<CourrierDestinataire> listeCourrierSecDir(Utilisateur employe);
    }
}
