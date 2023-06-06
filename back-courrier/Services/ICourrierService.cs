using back_courrier.Models;

namespace back_courrier.Services
{
    public interface ICourrierService
    {
        Courrier CreationCourrier(Courrier courrier, Utilisateur employe, List<Departement> destinataires, IFormFile formFile);
        IQueryable<Historique> ListeCourrierBaseQuery(int pageNumber, int pageSize, Boolean pagination);
        IList<Historique> ListeCourrierReceptionniste(int pageNumber, int pageSize, Boolean pagination);
        IList<Historique> ListeCourrierCoursier(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IList<Historique> ListeCourrierSec(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IList<Historique> ListeCourrierDir(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IList<Historique> ListeCourrier(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        Historique GetHistoriqueByIdCourrierDestinataire(int IdCourrierDestinataire);
        Historique TransfertCourrier(Historique historique);
        /*        int CalculateTotalPages(IList<Historique> listeCourrier, int pageSize);*/
        byte[] ExportPDF(IList<Historique> historiques);
    }
}
