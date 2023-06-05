using back_courrier.Models;

namespace back_courrier.Services
{
    public interface ICourrierService
    {
        Courrier creationCourrier(Courrier courrier, Utilisateur employe, List<Departement> destinataires, IFormFile formFile);
    }
}
