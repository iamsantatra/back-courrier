using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class CourrierDestinataire : BaseModel
    {
        [ForeignKey("Courrier")]
        public int IdCourrier { get; set; }
        public Courrier? Courrier { get; set; }
        [ForeignKey("Departement")]
        public int IdDepartementDestinataire { get; set; }
        public Departement? DepartementDestinataire { get; set; }
        [ForeignKey("Statut")]
        public int IdStatut { get; set; }
        public Statut? Statut { get; set; }
        [ForeignKey("Utilisateur")]
        public int IdResponsable { get; set; }
        public Utilisateur? Responsable { get; set; }
        public DateTime? DateMaj { get; set; } = DateTime.Now;
        public ICollection<Historique> Historiques { get; set; }
        public CourrierDestinataire() { }
        public CourrierDestinataire(Courrier courrier, Departement destinataire, Statut statut, Utilisateur responsable)
        {
            this.Courrier = courrier;
            this.DepartementDestinataire = destinataire;
            this.Responsable = responsable;
            this.Statut = statut;
        }
    }
}
