using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Historique : BaseModel
    {
        public virtual CourrierDestinataire CourrierDestinataire { get; set; }
        public virtual Statut Statut { get; set; }
        [Required]
        public DateTime DateHistorique { get; set; }
        [Required]
        public int CourrierDestinataireId { get; set; }
        [Required]
        public int StatutId { get; set; }

    }
}
