using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Historique : BaseModel
    {
        /*    Id INT NOT NULL IDENTITY(1,1),
            IdCourrierDestinataire INT not null,
            IdStatut INT not null,
            DateHistorique DATETIME null,*/
        [ForeignKey("CourrierDestinataire")]
        public int IdCourrierDestinataire { get; set; }
        [ForeignKey("Statut")]
        public int IdStatut { get; set; }
        [Required]
        public DateTime DateHistorique { get; set; }
        [ForeignKey("Utilisateur")]
        public int IdResponsable { get; set; }

    }
}
