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
        public virtual Poste Poste { get; set; }
        public virtual Departement Departement { get; set; }
        [Required]
        public int PosteId { get; set; }
        [Required]
        public int DepartementId { get; set; }
        // override toString method
        public override string ToString()
        {
            return "Utilisateur{" +
                    "nom='" + Nom + '\'' +
                    ", motDePasse='" + MotDePasse + '\'' +
                    ", poste=" + Poste +
                    ", departement=" + Departement +
                    '}';
        }

    }
}
