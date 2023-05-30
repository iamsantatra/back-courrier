using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Historique
    {
        /*    Id INT NOT NULL IDENTITY(1,1),
            IdCourrierDestinataire INT not null,
            IdStatut INT not null,
            DateHistorique DATETIME null,*/
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("CourrierDestinataire")]
        public int IdCourrierDestinataire { get; set; }
        [ForeignKey("Statut")]
        public int IdStatut { get; set; }
        [Required]
        public DateTime DateHistorique { get; set; }

    }
}
