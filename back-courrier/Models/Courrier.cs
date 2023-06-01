using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Courrier : BaseModel {
        /*        Id INT NOT NULL IDENTITY(1,1), 
                    Reference VARCHAR(40),
                    Objet VARCHAR(40) not null, 
                    DateCreation DATETIME not null,
                    Expediteur VARCHAR(40) not null,
                    Flag VARCHAR(20) not null, 
                    Commentaire TEXT not null,
                    IdReceptionniste INT not null,
                    -- Fichier FILE not null,
                    CONSTRAINT CK_priorite CHECK(Flag= 'urgent' OR Flag = 'normal' OR Flag = 'important'),
                    CONSTRAINT FK_Courrier_Receptionniste_IdUtilisateur FOREIGN KEY(IdReceptionniste) REFERENCES Utilisateur(Id),
                    CONSTRAINT PK_courrier PRIMARY KEY(Id)*/
        public string Reference { get; set; }

        [Required]
        public string Objet { get; set; }
        [Required]
        public DateTime DateCreation { get; set; }

        [Required]
        public string Expediteur { get; set; }

        [Required]
        public string Flag { get; set; }

        [Required]
        public string Commentaire { get; set; }

        [ForeignKey("Utilisateur")]
        public int IdReceptionniste { get; set; }

        // override toString
        public override string ToString()
        {
            return "Id: " + Id + " Reference: " + Reference + " Objet: " + Objet + " DateCreation: " + DateCreation + " Expediteur: " + Expediteur + " Flag: " + Flag + " Commentaire: " + Commentaire + " IdReceptionniste: " + IdReceptionniste;
        }

    }
}
