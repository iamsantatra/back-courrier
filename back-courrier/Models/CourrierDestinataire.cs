using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class CourrierDestinataire : BaseModel {
        public virtual Courrier Courrier { get; set; }
        public virtual Departement DepartementDestinataire { get; set; }
        [Required]
        public int DepartementDestinataireId { get; set; }
        [Required]
        public int CourrierId { get; set; }
    }
}
