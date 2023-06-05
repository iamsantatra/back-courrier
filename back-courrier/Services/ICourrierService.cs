﻿using back_courrier.Models;

namespace back_courrier.Services
{
    public interface ICourrierService
    {
        Courrier CreationCourrier(Courrier courrier, Utilisateur employe, List<Departement> destinataires, IFormFile formFile);
        IQueryable<Historique> ListeCourrierBaseQuery();
        IList<Historique> ListeCourrierReceptionniste();
        IList<Historique> ListeCourrierCoursier(Utilisateur employe);
        IList<Historique> ListeCourrierSecDir(Utilisateur employe);
        IList<Historique> ListeCourrier(Utilisateur employe);
    }
}
