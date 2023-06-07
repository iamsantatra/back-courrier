using back_courrier.Models;

namespace back_courrier.Services
{
    public interface ICourrierService
    {
        Courrier CreationCourrier(Courrier courrier, Utilisateur employe, List<Departement> destinataires, IFormFile formFile);
        IQueryable<Historique> ListeCourrierBaseQuery(int pageNumber, int pageSize, Boolean pagination);
        IQueryable<Historique> ListeCourrierReceptionnisteQuery(int pageNumber, int pageSize, Boolean pagination);
        IQueryable<Historique> ListeCourrierCoursierQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IQueryable<Historique> ListeCourrierSecQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IQueryable<Historique> ListeCourrierDirQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IQueryable<Historique> ListeCourrierQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        Historique GetHistoriqueByIdCourrierDestinataire(int IdCourrierDestinataire);
        Historique TransfertCourrier(Historique historique);
        /*        int CalculateTotalPages(IList<Historique> listeCourrier, int pageSize);*/
        byte[] ExportPDF(IList<Historique> historiques);
        IQueryable<Historique> QuerySearchBuilder(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            Historique historique, Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IList<Historique> ListeRecherche(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            Historique historique, Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);

        IList<Historique> ListeCourrier(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
    }
}
