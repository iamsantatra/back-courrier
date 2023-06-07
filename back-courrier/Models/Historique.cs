using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Historique : BaseModel
    {
        [ForeignKey("CourrierDestinataire")]
        public int IdCourrierDestinataire { get; set; }
        public CourrierDestinataire? CourrierDestinataire { get; set; }
        [ForeignKey("Statut")]
        public int IdStatut { get; set; }
        public Statut? Statut { get; set; }
        public DateTime? DateHistorique { get; set; } = DateTime.Now;
        [ForeignKey("Utilisateur")]
        public int IdResponsable { get; set; }
        public Utilisateur? Responsable { get; set; } 

        public Historique() { }
        public Historique(CourrierDestinataire courrierDestinataire, Statut statut, Utilisateur responsable)
        {
            this.CourrierDestinataire = courrierDestinataire;
            this.Statut = statut;
            this.Responsable = responsable;
        }
    }
}
