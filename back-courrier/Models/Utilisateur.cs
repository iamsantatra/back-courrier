using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Utilisateur
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string MotDePasse { get; set; }
        public int Id_Poste { get; set; }
        public int Id_Departement { get; set; }
    }
}
