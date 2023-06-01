using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Utilisateur : BaseModel
    {
        [Required]
        public string Nom { get; set; }
        [Required]
        public string MotDePasse { get; set; }
        [ForeignKey("Poste")]
        public int IdPoste { get; set; }
/*        public Poste poste { get; set; }*/
        [ForeignKey("Departement")]
        public int IdDepartement { get; set; }
/*        public Departement Departement { get; set; }
*/    }
}
