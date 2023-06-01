using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Courrier : BaseModel {
        public string? Reference { get; set; }

        [Required]
        public string Objet { get; set; }
        [Required]
        public DateTime DateCreation { get; set; }

        [Required]
        public string Expediteur { get; set; }

        [Required]
        public string Flag { get; set; }

        [Required]
        public string Commentaire { get; set; }

        public virtual Utilisateur? Receptionniste { get; set; }
        [Required]
        public int ReceptionnisteId { get; set; }

        // override toString
        public override string ToString()
        {
            return "Id: " + Id + " Reference: " + Reference + " Objet: " + Objet + " DateCreation: " + DateCreation + " Expediteur: " + Expediteur + " Flag: " + Flag + " Commentaire: " + Commentaire + " Receptionniste: " + Receptionniste;
        }
    }
}
