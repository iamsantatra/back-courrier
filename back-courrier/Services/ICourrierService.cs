using back_courrier.Models;

namespace back_courrier.Services
{
    public interface ICourrierService
    {
        Courrier CreationCourrier(Courrier courrier, Utilisateur employe, List<Departement> destinataires, IFormFile formFile);
        IQueryable<CourrierDestinataire> ListeCourrierBaseQuery();
        IQueryable<CourrierDestinataire> ListeCourrierReceptionnisteQuery();
        IQueryable<CourrierDestinataire> ListeCourrierCoursierQuery(Utilisateur employe);
        IQueryable<CourrierDestinataire> ListeCourrierSecQuery(Utilisateur employe);
        IQueryable<CourrierDestinataire> ListeCourrierDirQuery(Utilisateur employe);
        IQueryable<CourrierDestinataire> ListeCourrierQuery(Utilisateur employe);
        CourrierDestinataire GetDetailsCourrier(int IdCourrierDestinataire);
        CourrierDestinataire TransfertCourrier(CourrierDestinataire courrierDestinataire);
        byte[] ExportPDF(IList<CourrierDestinataire> listeCourrier);
        IQueryable<CourrierDestinataire> QuerySearchBuilder(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            string reference, string objet, string expediteurExterne, string expediteurInterne,
            string nomResponsable, string destinataire, string commentaire, string fichier,
            string recepteur, string flag, string statut, Utilisateur employe);
        Pages<CourrierDestinataire> ListeRecherche(DateTime? DateCreationStart, DateTime? DateCreationEnd,
            string reference, string objet, string expediteurExterne, string expediteurInterne,
            string nomResponsable, string destinataire, string commentaire, string fichier,
            string recepteur, string flag, string statut, Utilisateur employe, int pageNumber, int pageSize);
        Pages<CourrierDestinataire> ListeCourrierPage(Utilisateur employe, int pageNumber, int pageSize);
        IQueryable<CourrierDestinataire> ListeCourrierQuerySansPage(Utilisateur employe);
        IQueryable<CourrierDestinataire> ListeCourrierBaseQueryPage(IQueryable<CourrierDestinataire> listeCourrierBaseQuery, int pageNumber, int pageSize);
        public Dictionary<string, int> GetStatCourrierFlag();
        public Dictionary<string, int> GetStatCourrierDestinataire();

        public CourrierDestinataire TransfertCoursier(CourrierDestinataire courrierDestinataire);
        public CourrierDestinataire TransfertSecretaire(CourrierDestinataire courrierDestinataire);
        public CourrierDestinataire TransfertDirecteur(CourrierDestinataire courrierDestinataire);

    }
}
