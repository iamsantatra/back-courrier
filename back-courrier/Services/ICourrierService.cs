using back_courrier.Models;

namespace back_courrier.Services
{
    public interface ICourrierService
    {
        Courrier CreationCourrier(Courrier courrier, Utilisateur employe, List<Departement> destinataires, IFormFile formFile);
        IQueryable<CourrierDestinataire> ListeCourrierBaseQuery(int pageNumber, int pageSize, Boolean pagination);
        IQueryable<CourrierDestinataire> ListeCourrierReceptionnisteQuery(int pageNumber, int pageSize, Boolean pagination);
        IQueryable<CourrierDestinataire> ListeCourrierCoursierQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IQueryable<CourrierDestinataire> ListeCourrierSecQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IQueryable<CourrierDestinataire> ListeCourrierDirQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IQueryable<CourrierDestinataire> ListeCourrierQuery(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        CourrierDestinataire GetDetailsCourrier(int IdCourrierDestinataire);
        Historique TransfertCourrier(CourrierDestinataire historique);
        /*        int CalculateTotalPages(IList<Historique> listeCourrier, int pageSize);*/
        byte[] ExportPDF(IList<CourrierDestinataire> listeCourrier);
        IQueryable<CourrierDestinataire> QuerySearchBuilder(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            CourrierDestinataire courrierDestinataire, Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
        IList<CourrierDestinataire> ListeRecherche(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            CourrierDestinataire courrierDestinataire, Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);

        IList<CourrierDestinataire> ListeCourrier(Utilisateur employe, int pageNumber, int pageSize, Boolean pagination);
    }
}
