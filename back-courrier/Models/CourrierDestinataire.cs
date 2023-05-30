using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class CourrierDestinataire {
        /*      Id INT NOT NULL IDENTITY(1,1),
                IdCourrier INT not null,
                IdDepartementDestinataire INT not null,*/
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Courrier")]
        public int IdCourrier { get; set; }
        [ForeignKey("Departement")]
        public int IdDepartementDestinataire { get; set; }
    }
}
