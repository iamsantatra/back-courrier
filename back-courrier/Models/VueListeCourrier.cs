using System.ComponentModel.DataAnnotations;

namespace back_courrier.Models
{
    public class VueListeCourrier
    {
        public int IdCourrier { get; set; }
        [Key]
        public int IdCourrierDestinataire { get; set; }
        public string Reference { get; set; }
        public string Objet { get; set; }
        public DateTime DateCreation { get; set; }
        public string Expediteur { get; set; }
        public string Flag { get; set; }
        public string Commentaire { get; set; }
        public int IdReceptionniste { get; set; }
        public string NomStatut { get; set; }
        public string NomDepartement { get; set; } 
        public int IdDepartement { get; set; }
        public int IdStatut { get; set; }
        public string NomResponsable { get; set; }
    }
}
