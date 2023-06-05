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
        public Departement? Departement { get; set; }
        public DateTime? DateMaj { get; set; } = DateTime.Now;
        public CourrierDestinataire() { }
        public CourrierDestinataire(Courrier courrier, Departement destinataire)
        {
            this.Courrier = courrier;
            this.Departement = destinataire;
        }
    }
}
