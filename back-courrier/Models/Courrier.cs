using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Courrier : BaseModel {
        public string? Reference { get; set; }

        [Required]
        public string Objet { get; set; }
        public DateTime? DateCreation { get; set; } = DateTime.Now;
        
        public string? ExpediteurExterne { get; set; }

        [ForeignKey("Flag")]
        public int IdFlag { get; set; }
        public Flag? Flag { get; set; }
        public string? Commentaire { get; set; }
        public string? Fichier { get; set; }

        [ForeignKey("Recepteur")]
        public int IdReceptionniste { get; set; }
        public Utilisateur? Recepteur { get; set; }
        [ForeignKey("ExpediteurInterne")]
        public int? IdExpediteur { get; set; }
        public Departement? ExpediteurInterne { get; set; }

        public ICollection<CourrierDestinataire>? Destinataires { get; set; }
    }
}
